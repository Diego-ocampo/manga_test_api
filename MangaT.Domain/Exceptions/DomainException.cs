namespace MangaT.Domain.Exceptions;

/// <summary>Excepción lanzada cuando se incumple una regla de negocio del dominio.</summary>
public class DomainException(string message) : Exception(message);
