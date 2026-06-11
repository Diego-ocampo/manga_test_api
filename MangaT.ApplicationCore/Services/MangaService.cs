using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Mapping;

namespace MangaT.ApplicationCore.Services;

public class MangaService(IMangaRepository mangaRepository) : IMangaService
{
    public async Task<IReadOnlyList<MangaDto>> GetPopularAsync(CancellationToken cancellationToken = default)
    {
        var mangas = await mangaRepository.GetPopularAsync(cancellationToken);
        return mangas.Select(m => m.ToDto()).ToList();
    }
}
