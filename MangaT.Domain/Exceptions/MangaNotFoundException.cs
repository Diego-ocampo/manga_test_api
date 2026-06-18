namespace MangaT.Domain.Exceptions;

/// <summary>
/// Excepción de dominio: el manga solicitado no existe o no es visible para el usuario.
/// Se lanza desde ApplicationCore y se traduce a HTTP 404 en GlobalExceptionHandler.
/// </summary>
public class MangaNotFoundException(int id)
    : DomainException($"No se encontró el manga con id {id}.");
