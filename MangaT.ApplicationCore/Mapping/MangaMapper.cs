using MangaT.ApplicationCore.DTOs;
using MangaT.Domain.Entities;

namespace MangaT.ApplicationCore.Mapping;

public static class MangaMapper
{
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
