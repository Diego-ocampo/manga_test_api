using MangaT.ApplicationCore.DTOs;

namespace MangaT.ApplicationCore.Interfaces;

public interface IMangaService
{
    Task<IReadOnlyList<MangaDto>> GetPopularAsync(CancellationToken cancellationToken = default);
}
