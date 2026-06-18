using MangaT.ApplicationCore.Common;
using MangaT.ApplicationCore.DTOs;

namespace MangaT.ApplicationCore.Interfaces;

/// <summary>Contrato de casos de uso expuestos por la API de mangas.</summary>
public interface IMangaService
{
    /// <param name="category">Filtro opcional por categoría (?category=Action).</param>
    Task<PagedResult<MangaDto>> GetPagedAsync(
        int page,
        int pageSize,
        string? category = null,
        CancellationToken cancellationToken = default);

    Task<PagedResult<MangaDto>> GetPopularAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<MangaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MangaDto> CreateAsync(CreateMangaRequest request, CancellationToken cancellationToken = default);
    Task<MangaDto> UpdateAsync(int id, UpdateMangaRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
