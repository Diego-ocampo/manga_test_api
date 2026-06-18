using MangaT.ApplicationCore.Exceptions;
using MangaT.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MangaT.API.ExceptionHandling;

/// <summary>
/// Captura excepciones globales y las traduce a respuestas ProblemDetails (JSON).
/// </summary>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problem = CreateProblemDetails(httpContext, exception);

        // Solo registramos errores 5xx; 4xx son errores esperados del cliente.
        if (problem.Status >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Error no controlado");
        }

        httpContext.Response.StatusCode = problem.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    /// <summary>Construye ProblemDetails con código de error y traceId para trazabilidad.</summary>
    private ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        var (statusCode, title, errorCode, detail) = MapException(exception);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = $"https://httpstatuses.com/{statusCode}",
            Instance = httpContext.Request.Path
        };

        problem.Extensions["errorCode"] = errorCode;
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        return problem;
    }

    /// <summary>Mapea tipos de excepción de dominio/aplicación a códigos HTTP.</summary>
    private (int StatusCode, string Title, string ErrorCode, string Detail) MapException(Exception exception) =>
        exception switch
        {
            AppException app => (app.StatusCode, app.Title, app.ErrorCode, app.Message),
            MangaNotFoundException notFound => (
                StatusCodes.Status404NotFound,
                "Recurso no encontrado",
                "NOT_FOUND",
                notFound.Message),
            DomainException domain => (
                StatusCodes.Status400BadRequest,
                "Regla de negocio incumplida",
                "DOMAIN_ERROR",
                domain.Message),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Error del servidor",
                "INTERNAL_ERROR",
                environment.IsDevelopment()
                    ? exception.Message
                    : "Ocurrió un error interno.")
        };
}
