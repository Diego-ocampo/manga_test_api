using FluentValidation;
using MangaT.ApplicationCore.Common;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Exceptions;
using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Mapping;
using MangaT.Domain.Entities;
using MangaT.Domain.ValueObjects;

namespace MangaT.ApplicationCore.Services;

/// <summary>
/// Orquesta casos de uso de mangas: validación, reglas de dominio y persistencia.
/// </summary>
public class MangaService(
    IMangaRepository mangaRepository,
    IValidator<CreateMangaRequest> createValidator,
    IValidator<UpdateMangaRequest> updateValidator) : IMangaService
{
    /// <inheritdoc />
    public async Task<PagedResult<MangaDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = await mangaRepository.GetPagedAsync(NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);
        return MapPagedResult(result);
    }

    /// <inheritdoc />
    public async Task<PagedResult<MangaDto>> GetPopularAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = await mangaRepository.GetPopularAsync(NormalizePage(page), NormalizePageSize(pageSize), cancellationToken);
        return MapPagedResult(result);
    }

    /// <inheritdoc />
    public async Task<MangaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var manga = await mangaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw AppException.NotFound($"No se encontró el manga con id {id}.");

        return manga.ToDto();
    }

    /// <inheritdoc />
    public async Task<MangaDto> CreateAsync(CreateMangaRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(createValidator, request, cancellationToken);

        var manga = Manga.Create(
            request.Title,
            request.Author,
            request.Description,
            request.Category,
            request.VolumeCount,
            new Rating(request.Point),
            request.ImageUrl,
            request.DetailUrl);

        await mangaRepository.AddAsync(manga, cancellationToken);
        return manga.ToDto();
    }

    /// <inheritdoc />
    public async Task<MangaDto> UpdateAsync(int id, UpdateMangaRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(updateValidator, request, cancellationToken);

        var manga = await mangaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw AppException.NotFound($"No se encontró el manga con id {id}.");

        manga.Update(
            request.Title,
            request.Author,
            request.Description,
            request.Category,
            request.VolumeCount,
            new Rating(request.Point),
            request.ImageUrl,
            request.DetailUrl);

        await mangaRepository.UpdateAsync(manga, cancellationToken);
        return manga.ToDto();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var manga = await mangaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw AppException.NotFound($"No se encontró el manga con id {id}.");

        await mangaRepository.DeleteAsync(manga, cancellationToken);
    }

    /// <summary>Convierte entidades de dominio paginadas a DTOs para la API.</summary>
    private static PagedResult<MangaDto> MapPagedResult(PagedResult<Manga> result) => new()
    {
        Items = result.Items.Select(m => m.ToDto()).ToList(),
        Page = result.Page,
        PageSize = result.PageSize,
        TotalCount = result.TotalCount
    };

    /// <summary>Ejecuta FluentValidation y lanza AppException si hay errores.</summary>
    private static async Task ValidateAsync<T>(IValidator<T> validator, T request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            return;
        }

        var error = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
        throw AppException.Validation(error);
    }

    /// <summary>Página mínima 1 para evitar offsets negativos en SQL.</summary>
    private static int NormalizePage(int page) => page < 1 ? 1 : page;

    /// <summary>Limita el tamaño de página entre 1 y 50 registros.</summary>
    private static int NormalizePageSize(int pageSize) => pageSize switch
    {
        < 1 => 10,
        > 50 => 50,
        _ => pageSize
    };
}
