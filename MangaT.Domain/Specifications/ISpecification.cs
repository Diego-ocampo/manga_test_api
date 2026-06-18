using System.Linq.Expressions;

namespace MangaT.Domain.Specifications;

/// <summary>
/// Patrón Specification (DDD): encapsula una regla de consulta reutilizable y combinable.
/// SOLID — Open/Closed: nuevos filtros = nueva clase, sin modificar el repositorio.
/// </summary>
/// <typeparam name="T">Tipo de entidad filtrada (ej. Manga).</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Expresión LINQ que EF Core traduce a SQL (WHERE en la base de datos).
    /// </summary>
    Expression<Func<T, bool>> ToExpression();
}
