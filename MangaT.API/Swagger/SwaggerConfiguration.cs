using Microsoft.OpenApi;

namespace MangaT.API.Swagger;

/// <summary>
/// Configuración de Swagger UI y Swashbuckle.
/// </summary>
/// <remarks>
/// <para><b>ESTILO CLÁSICO (C# 3–13):</b> extension methods con parámetro <c>this</c>.</para>
/// <para>Este archivo se mantiene a propósito para comparar con C# 14 (<c>extension</c> blocks)
/// en <c>SerilogExtensions</c>, <c>DependencyInjection</c> y <c>MangaMapper</c>.</para>
/// <para><b>Preguntas de repaso:</b> ¿Qué hace el modificador <c>this</c>? ¿En qué tipo de clase deben vivir?</para>
/// </remarks>
public static class SwaggerConfiguration
{
    /// <summary>Registra generación de documento Swagger con soporte JWT Bearer.</summary>
    public static IServiceCollection AddMangaTSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MangaT API",
                Version = "v1",
                Description = "Documentación Swagger (estándar de la industria). También disponible: Scalar UI en /scalar/v1 y OpenAPI 3.1 nativo en /openapi/v1.json"
            });

            // Botón "Authorize" en Swagger UI para probar endpoints protegidos.
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingresa el token JWT obtenido desde POST /api/v1/auth/login"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });

        return services;
    }

    /// <summary>Expone Swagger UI en /swagger y el JSON en /swagger/v1/swagger.json.</summary>
    public static WebApplication UseMangaTSwagger(this WebApplication app)
    {
        app.UseSwagger(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
        });
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "MangaT API v1 (Swagger)");
            options.RoutePrefix = "swagger";
            options.DocumentTitle = "MangaT API — Swagger UI";
        });

        return app;
    }
}
