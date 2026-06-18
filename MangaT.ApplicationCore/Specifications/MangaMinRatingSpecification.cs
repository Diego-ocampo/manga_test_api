using System.Linq.Expressions;
using MangaT.Domain.Entities;
using MangaT.Domain.Specifications;

namespace MangaT.ApplicationCore.Specifications;

/// <summary>
/// Specification concreta: mangas con calificación mínima (Point &gt;= umbral).
/// Usada para la regla de negocio del rol Reader (solo ve mangas con Point &gt;= 7).
/// </summary>
public sealed class MangaMinRatingSpecification(double minimumPoint) : ISpecification<Manga>
{
    /// <inheritdoc />
    public Expression<Func<Manga, bool>> ToExpression() =>
        manga => manga.Point >= minimumPoint;
}
