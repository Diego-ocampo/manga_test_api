using MangaT.ApplicationCore.Common;
using MangaT.Domain.Entities;

namespace MangaT.ApplicationCore.Interfaces;

/// <summary>Abstracción de persistencia de mangas (implementada por EF Core).</summary>
public interface IMangaRepository
{
    Task<PagedResult<Manga>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Manga>> GetPopularAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Manga?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Manga manga, CancellationToken cancellationToken = default);
    Task UpdateAsync(Manga manga, CancellationToken cancellationToken = default);
    Task DeleteAsync(Manga manga, CancellationToken cancellationToken = default);
}
