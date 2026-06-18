using MangaT.ApplicationCore.Common;
using MangaT.ApplicationCore.DTOs;

namespace MangaT.ApplicationCore.Interfaces;

public interface IMangaService
{
    Task<PagedResult<MangaDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<MangaDto>> GetPopularAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<MangaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MangaDto> CreateAsync(CreateMangaRequest request, CancellationToken cancellationToken = default);
    Task<MangaDto> UpdateAsync(int id, UpdateMangaRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
