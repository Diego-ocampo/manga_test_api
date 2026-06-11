using MangaT.ApplicationCore.Entities;

namespace MangaT.ApplicationCore.Interfaces;

public interface IMangaRepository
{
    Task<IReadOnlyList<Manga>> GetPopularAsync(CancellationToken cancellationToken = default);
}
