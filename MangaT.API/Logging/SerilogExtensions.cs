using Serilog;
using Serilog.Events;

namespace MangaT.API.Logging;

/// <summary>
/// Configuración centralizada de Serilog para MangaT API.
/// Lee sinks y niveles desde appsettings y enriquece cada evento con contexto.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Logger mínimo de arranque: captura fallos antes de cargar appsettings completo.
    /// </summary>
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateBootstrapLogger();
    }

    /// <summary>
    /// Integra Serilog con el host de ASP.NET Core (reemplaza el logging por defecto).
    /// </summary>
    public static WebApplicationBuilder AddMangaTSerilog(this WebApplicationBuilder builder)
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

    /// <summary>
    /// Registra cada petición HTTP con duración, status y TraceId (correlación con ProblemDetails).
    /// </summary>
    public static WebApplication UseMangaTRequestLogging(this WebApplication app)
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
