using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Entities;

namespace MangaT.ApplicationCore.Mapping;

public static class MangaMapper
{
    public static MangaDto ToDto(this Manga manga) => new()
    {
        Title = manga.Title,
        Author = manga.Author,
        Description = manga.Description,
        Category = manga.Category,
        VolumeCount = manga.VolumeCount,
        Point = manga.Point,
        ImageUrl = new Uri(manga.ImageUrl),
        DetailUrl = new Uri(manga.DetailUrl)
    };
}
