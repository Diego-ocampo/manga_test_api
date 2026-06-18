using System.Linq.Expressions;
using MangaT.Domain.Entities;
using MangaT.Domain.Specifications;

namespace MangaT.ApplicationCore.Specifications;

/// <summary>
/// Specification concreta: filtra mangas por categoría (ej. Action, Adventure).
/// Ejemplo de Open/Closed — añadir filtros sin tocar <see cref="IMangaRepository"/>.
/// </summary>
public sealed class MangaByCategorySpecification(string category) : ISpecification<Manga>
{
    private readonly string _normalizedCategory = category.Trim();

    /// <inheritdoc />
    public Expression<Func<Manga, bool>> ToExpression() =>
        manga => manga.Category == _normalizedCategory;
}
