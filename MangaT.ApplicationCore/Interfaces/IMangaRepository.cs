using MangaT.ApplicationCore.Common;
using MangaT.Domain.Entities;
using MangaT.Domain.Specifications;

namespace MangaT.ApplicationCore.Interfaces;

/// <summary>
/// Abstracción de persistencia de mangas (implementada por EF Core).
/// Interface Segregation: solo operaciones de persistencia, sin lógica HTTP.
/// </summary>
public interface IMangaRepository
{
    /// <summary>
    /// Consulta paginada con specifications opcionales (Open/Closed).
    /// Cada ISpecification se traduce a un WHERE en SQL.
    /// </summary>
    Task<PagedResult<Manga>> QueryAsync(
        int page,
        int pageSize,
        IReadOnlyList<ISpecification<Manga>> specifications,
        MangaSortOrder sortOrder,
        CancellationToken cancellationToken = default);

    Task<Manga?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca por id aplicando specifications (ej. Reader no puede ver Point &lt; 7).
    /// </summary>
    Task<Manga?> GetByIdAsync(
        int id,
        IReadOnlyList<ISpecification<Manga>> specifications,
        CancellationToken cancellationToken = default);

    Task AddAsync(Manga manga, CancellationToken cancellationToken = default);
    Task UpdateAsync(Manga manga, CancellationToken cancellationToken = default);
    Task DeleteAsync(Manga manga, CancellationToken cancellationToken = default);
}
