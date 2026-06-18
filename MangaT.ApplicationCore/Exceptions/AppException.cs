namespace MangaT.ApplicationCore.Exceptions;

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

    public int StatusCode { get; }
    public string Title { get; }
    public string ErrorCode { get; }
    public string Type { get; }

    public static AppException NotFound(string message) =>
        new(message, 404, "Recurso no encontrado", "NOT_FOUND");

    public static AppException Validation(string message) =>
        new(message, 422, "Error de validación", "VALIDATION_ERROR");

    public static AppException Business(string message) =>
        new(message, 400, "Regla de negocio incumplida", "DOMAIN_ERROR");
}
