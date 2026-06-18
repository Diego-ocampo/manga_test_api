using FluentValidation;
using MangaT.ApplicationCore.Common;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Exceptions;
using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Mapping;
using MangaT.ApplicationCore.Specifications;
using MangaT.Domain.Entities;
using MangaT.Domain.Exceptions;
using MangaT.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MangaT.ApplicationCore.Services;

/// <summary>
/// Orquesta casos de uso de mangas: validación, reglas de dominio, specifications y persistencia.
/// Single Responsibility: coordina; no construye SQL ni conoce HttpContext.
/// </summary>
public class MangaService(
    IMangaRepository mangaRepository,
    ICurrentUserContext currentUser,
    IValidator<CreateMangaRequest> createValidator,
    IValidator<UpdateMangaRequest> updateValidator,
    ILogger<MangaService> logger) : IMangaService
{
    /// <inheritdoc />
    public async Task<PagedResult<MangaDto>> GetPagedAsync(
        int page,
        int pageSize,
        string? category = null,
        CancellationToken cancellationToken = default)
    {
        var specifications = MangaQuerySpecifications.BuildForList(currentUser, category);

        var result = await mangaRepository.QueryAsync(
            NormalizePage(page),
            NormalizePageSize(pageSize),
            specifications,
            MangaSortOrder.TitleAsc,
            cancellationToken);

        return MapPagedResult(result);
    }

    /// <inheritdoc />
    public async Task<PagedResult<MangaDto>> GetPopularAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        // Misma regla Reader que en listado, pero ordenado por calificación.
        var specifications = MangaQuerySpecifications.BuildForList(currentUser, category: null);

        var result = await mangaRepository.QueryAsync(
            NormalizePage(page),
            NormalizePageSize(pageSize),
            specifications,
            MangaSortOrder.PointDesc,
            cancellationToken);

        return MapPagedResult(result);
    }

    /// <inheritdoc />
    public async Task<MangaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var specifications = MangaQuerySpecifications.BuildForSingle(currentUser);

        var manga = await mangaRepository.GetByIdAsync(id, specifications, cancellationToken)
            ?? throw new MangaNotFoundException(id);

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

        logger.LogInformation(
            "Manga creado {MangaId} — {Title} por {Author}",
            manga.Id,
            manga.Title,
            manga.Author);

        return manga.ToDto();
    }

    /// <inheritdoc />
    public async Task<MangaDto> UpdateAsync(int id, UpdateMangaRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(updateValidator, request, cancellationToken);

        var manga = await mangaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new MangaNotFoundException(id);

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

        logger.LogInformation(
            "Manga actualizado {MangaId} — {Title}",
            manga.Id,
            manga.Title);

        return manga.ToDto();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var manga = await mangaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new MangaNotFoundException(id);

        await mangaRepository.DeleteAsync(manga, cancellationToken);

        logger.LogInformation(
            "Manga eliminado {MangaId} — {Title}",
            manga.Id,
            manga.Title);
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
