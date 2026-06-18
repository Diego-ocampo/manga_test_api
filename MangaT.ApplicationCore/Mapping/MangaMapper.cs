using MangaT.ApplicationCore.DTOs;
using MangaT.Domain.Entities;

namespace MangaT.ApplicationCore.Mapping;

/// <summary>
/// Extension members C# 14: <c>manga.ToDto()</c> se lee como método de instancia de <see cref="Manga"/>.
/// </summary>
public static class MangaMapper
{
    extension(Manga manga)
    {
        /// <summary>Mapea una entidad <see cref="Manga"/> a su representación pública.</summary>
        public MangaDto ToDto() => new()
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
}
