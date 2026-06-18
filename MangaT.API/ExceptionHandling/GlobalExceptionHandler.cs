using MangaT.ApplicationCore.Exceptions;
using MangaT.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MangaT.API.ExceptionHandling;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problem = CreateProblemDetails(httpContext, exception);

        if (problem.Status >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Error no controlado");
        }

        httpContext.Response.StatusCode = problem.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

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
