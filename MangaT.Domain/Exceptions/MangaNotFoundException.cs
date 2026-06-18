namespace MangaT.Domain.Exceptions;

/// <summary>Excepción específica cuando no existe un manga con el id solicitado.</summary>
public class MangaNotFoundException(int id)
    : DomainException($"No se encontró el manga con id {id}.");
