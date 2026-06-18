using MangaT.ApplicationCore.Interfaces;
using MangaT.Domain.Entities;
using MangaT.Domain.Specifications;

namespace MangaT.ApplicationCore.Specifications;

/// <summary>
/// Factory de specifications de lectura según el contexto del usuario (rol, filtros HTTP).
/// Single Responsibility: centraliza QUÉ filtros aplicar; el servicio solo orquesta.
/// </summary>
public static class MangaQuerySpecifications
{
    /// <summary>Calificación mínima visible para el rol Reader (ejercicio Fase 4).</summary>
    public const double ReaderMinimumPoint = 7.0;

    /// <summary>
    /// Construye la lista de specifications activas para consultas de listado.
    /// </summary>
    /// <param name="currentUser">Contexto del usuario autenticado (o anónimo).</param>
    /// <param name="category">Filtro opcional desde query string (?category=Action).</param>
    public static IReadOnlyList<ISpecification<Manga>> BuildForList(
        ICurrentUserContext currentUser,
        string? category)
    {
        var specifications = new List<ISpecification<Manga>>();

        if (!string.IsNullOrWhiteSpace(category))
        {
            specifications.Add(new MangaByCategorySpecification(category));
        }

        // Regla de negocio por rol: Reader tiene catálogo restringido; Admin y anónimos ven todo.
        if (currentUser.IsInRole("Reader"))
        {
            specifications.Add(new MangaMinRatingSpecification(ReaderMinimumPoint));
        }

        return specifications;
    }

    /// <summary>
    /// Specifications para lectura de un manga por id (misma regla de rating para Reader).
    /// </summary>
    public static IReadOnlyList<ISpecification<Manga>> BuildForSingle(
        ICurrentUserContext currentUser)
    {
        if (!currentUser.IsInRole("Reader"))
        {
            return [];
        }

        return [new MangaMinRatingSpecification(ReaderMinimumPoint)];
    }
}
