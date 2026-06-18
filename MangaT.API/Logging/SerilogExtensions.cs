using Serilog;
using Serilog.Events;

namespace MangaT.API.Logging;

/// <summary>
/// Configuración centralizada de Serilog para MangaT API.
/// Usa bloques <c>extension</c> de C# 14 (.NET 10).
/// Comparar con <see cref="Swagger.SwaggerConfiguration"/> que conserva el estilo clásico con <c>this</c>.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Logger mínimo de arranque: captura fallos antes de cargar appsettings completo.
    /// (Método estático normal, no es extension method.)
    /// </summary>
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateBootstrapLogger();
    }

    // C# 14: bloque extension — el receptor es WebApplicationBuilder (equivalente a `this WebApplicationBuilder`).
    extension(WebApplicationBuilder builder)
    {
        /// <summary>Integra Serilog con el host de ASP.NET Core (reemplaza el logging por defecto).</summary>
        public WebApplicationBuilder AddMangaTSerilog()
        {
            builder.Host.UseSerilog((context, _, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "MangaT.API");

                // Tests: silenciar ruido en consola salvo errores fatales.
                if (context.HostingEnvironment.IsEnvironment("Testing"))
                {
                    loggerConfiguration.MinimumLevel.Fatal();
                }
            });

            return builder;
        }
    }

    extension(WebApplication app)
    {
        /// <summary>Registra cada petición HTTP con duración, status y TraceId (correlación con ProblemDetails).</summary>
        public WebApplication UseMangaTRequestLogging()
        {
            if (app.Environment.IsEnvironment("Testing"))
            {
                return app;
            }

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} → {StatusCode} en {Elapsed:0.0000} ms";

                options.GetLevel = (httpContext, elapsed, ex) =>
                {
                    if (ex is not null || httpContext.Response.StatusCode >= 500)
                    {
                        return LogEventLevel.Error;
                    }

                    if (httpContext.Response.StatusCode >= 400)
                    {
                        return LogEventLevel.Warning;
                    }

                    // Endpoints de health check: menos ruido en consola.
                    if (httpContext.Request.Path.StartsWithSegments("/health"))
                    {
                        return LogEventLevel.Debug;
                    }

                    return LogEventLevel.Information;
                };

                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
                    diagnosticContext.Set("User", httpContext.User.Identity?.Name ?? "anonymous");
                    diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
                };
            });

            return app;
        }
    }
}
