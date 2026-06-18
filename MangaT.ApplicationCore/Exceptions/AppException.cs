namespace MangaT.ApplicationCore.Exceptions;

/// <summary>
/// Excepción de aplicación con metadatos HTTP para el manejador global.
/// </summary>
public class AppException : Exception
{
    private AppException(string message, int statusCode, string title, string errorCode, string? type = null)
        : base(message)
    {
        StatusCode = statusCode;
        Title = title;
        ErrorCode = errorCode;
        Type = type ?? $"https://httpstatuses.com/{statusCode}";
    }

    /// <summary>Código HTTP que devolverá la API.</summary>
    public int StatusCode { get; }

    /// <summary>Título legible del error (ProblemDetails.Title).</summary>
    public string Title { get; }

    /// <summary>Código estable para clientes (p. ej. NOT_FOUND, VALIDATION_ERROR).</summary>
    public string ErrorCode { get; }

    /// <summary>URI de referencia del tipo de error.</summary>
    public string Type { get; }

    /// <summary>Recurso no encontrado (404).</summary>
    public static AppException NotFound(string message) =>
        new(message, 404, "Recurso no encontrado", "NOT_FOUND");

    /// <summary>Datos de entrada inválidos (422).</summary>
    public static AppException Validation(string message) =>
        new(message, 422, "Error de validación", "VALIDATION_ERROR");

    /// <summary>Regla de negocio incumplida (400).</summary>
    public static AppException Business(string message) =>
        new(message, 400, "Regla de negocio incumplida", "DOMAIN_ERROR");
}
