namespace MangaT.Domain.Exceptions;

public class MangaNotFoundException(int id)
    : DomainException($"No se encontró el manga con id {id}.");
