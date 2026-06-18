using System.Text;
using MangaT.ApplicationCore;
using MangaT.ApplicationCore.Interfaces;
using MangaT.Infrastructure;
using MangaT.Infrastructure.Persistence;
using MangaT.Infrastructure.Repositories;
using MangaT.Domain.Entities;
using MangaT.Domain.ValueObjects;
using MangaT.API.ExceptionHandling;
using MangaT.API.OpenApi;
using MangaT.API.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

// Punto de entrada de la API REST de MangaT.
// Configura servicios, autenticación JWT, persistencia y el pipeline HTTP.
var builder = WebApplication.CreateBuilder(args);

// Respuestas de error estandarizadas (RFC 7807 Problem Details).
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

// Convierte excepciones no capturadas en ProblemDetails JSON.
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

// OpenAPI 3.1 nativo de ASP.NET Core + transformadores para documentar JWT.
builder.Services.AddTransient<BearerSecuritySchemeTransformer>();
builder.Services.AddOpenApi("v1", options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<AuthorizeOperationTransformer>();
});
// Swagger UI clásico (complementa Scalar y el endpoint /openapi/v1.json).
builder.Services.AddMangaTSwagger();

// Autenticación JWT: clave simétrica leída desde appsettings / variables de entorno.
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

// En tests de integración usamos base de datos en memoria; en producción, SQL Server.
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

// Registra servicios de aplicación (MangaService, AuthService, validadores).
builder.Services.AddApplication();

var app = builder.Build();

// Aplica migraciones EF Core al arrancar (con reintentos para Docker/SQL Server).
if (!app.Environment.IsEnvironment("Testing"))
{
    await MigrateDatabaseWithRetryAsync(app.Services);
}
else
{
    // Entorno de pruebas: crea el esquema en memoria y siembra un manga de ejemplo.
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

// Documentación interactiva solo en desarrollo (no se expone en producción).
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseMangaTSwagger();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("MangaT API")
               .WithOpenApiRoutePattern("/openapi/{documentName}.json");
    });
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

/// <summary>
/// Ejecuta migraciones con reintentos: SQL Server puede no estar listo al iniciar Docker Compose.
/// </summary>
static async Task MigrateDatabaseWithRetryAsync(IServiceProvider services)
{
    const int maxRetries = 10;

    for (var attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MangaDbContext>();
            await context.Database.MigrateAsync();
            return;
        }
        catch (Exception) when (attempt < maxRetries)
        {
            // Espera 3 s antes del siguiente intento de conexión a la base de datos.
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}

public partial class Program;
