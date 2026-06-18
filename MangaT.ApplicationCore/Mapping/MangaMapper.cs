using MangaT.ApplicationCore.DTOs;
using MangaT.Domain.Entities;

namespace MangaT.ApplicationCore.Mapping;

/// <summary>
/// Extensiones para convertir entidades de dominio en DTOs de respuesta.
/// </summary>
public static class MangaMapper
{
    /// <summary>Mapea una entidad <see cref="Manga"/> a su representación pública.</summary>
    public static MangaDto ToDto(this Manga manga) => new()
    {
        Id = manga.Id,
        Title = manga.Title,
        Author = manga.Author,
        Description = manga.Description,
        Category = manga.Category,
        VolumeCount = manga.VolumeCount,
        Point = manga.Point,
        ImageUrl = manga.ImageUrl,
        DetailUrl = manga.DetailUrl,
        CreatedDate = manga.CreatedDate
    };
}
