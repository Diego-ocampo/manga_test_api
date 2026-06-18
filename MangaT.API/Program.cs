using System.Text;
using MangaT.ApplicationCore;
using MangaT.ApplicationCore.Interfaces;
using MangaT.Infrastructure;
using MangaT.Infrastructure.Persistence;
using MangaT.Infrastructure.Repositories;
using MangaT.Domain.Entities;
using MangaT.Domain.ValueObjects;
using MangaT.API.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

        if (!context.ProblemDetails.Extensions.ContainsKey("errorCode"))
        {
            context.ProblemDetails.Extensions["errorCode"] = "VALIDATION_ERROR";
        }
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var detail = string.Join(" ",
                context.ModelState.Values
                    .Where(value => value?.Errors.Count > 0)
                    .SelectMany(value => value!.Errors)
                    .Select(error => error.ErrorMessage));

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Error de validación",
                Detail = string.IsNullOrWhiteSpace(detail) ? "La solicitud contiene datos inválidos." : detail,
                Type = "https://httpstatuses.com/422",
                Instance = context.HttpContext.Request.Path
            };

            problem.Extensions["errorCode"] = "VALIDATION_ERROR";
            problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            return new ObjectResult(problem) { StatusCode = problem.Status };
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MangaT API",
        Version = "v1",
        Description = "API de ejemplo para aprender arquitectura limpia, DDD y seguridad."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT obtenido desde POST /api/v1/auth/login"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key no configurada.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<MangaDbContext>(options =>
        options.UseInMemoryDatabase("MangaTTests"));
    builder.Services.AddScoped<IMangaRepository, MangaRepository>();
    builder.Services.AddHealthChecks();
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddInfrastructure(connectionString);
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<MangaDbContext>("database");
}

builder.Services.AddApplication();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MangaDbContext>();
    await context.Database.MigrateAsync();
}
else
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MangaDbContext>();
    await context.Database.EnsureCreatedAsync();

    if (!await context.Mangas.AnyAsync())
    {
        await context.Mangas.AddAsync(Manga.Create(
            "One Piece",
            "Eiichiro Oda",
            "A story about pirates.",
            "Adventure",
            100,
            new Rating(9.5),
            "https://example.com/onepiece.jpg",
            "https://example.com/onepiece"));
        await context.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MangaT API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;
