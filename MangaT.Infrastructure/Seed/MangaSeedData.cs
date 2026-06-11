using MangaT.ApplicationCore.Entities;

namespace MangaT.Infrastructure.Seed;

public static class MangaSeedData
{
    private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static IReadOnlyList<Manga> GetMangas() =>
    [
        new()
        {
            Title = "One Piece",
            Author = "Eiichiro Oda",
            Description = "A story about a group of pirates searching for the ultimate treasure.",
            Category = "Adventure",
            VolumeCount = 100,
            Point = 9.5,
            ImageUrl = "https://example.com/onepiece.jpg",
            DetailUrl = "https://example.com/onepiece",
            CreatedDate = SeedDate
        },
        new()
        {
            Title = "Naruto",
            Author = "Masashi Kishimoto",
            Description = "A young ninja's journey to become the strongest in his village.",
            Category = "Action",
            VolumeCount = 72,
            Point = 9.0,
            ImageUrl = "https://example.com/naruto.jpg",
            DetailUrl = "https://example.com/naruto",
            CreatedDate = SeedDate
        },
        new()
        {
            Title = "Bleach",
            Author = "Tite Kubo",
            Description = "A teenager gains the powers of a Soul Reaper and battles evil spirits.",
            Category = "Fantasy",
            VolumeCount = 74,
            Point = 8.5,
            ImageUrl = "https://example.com/bleach.jpg",
            DetailUrl = "https://example.com/bleach",
            CreatedDate = SeedDate
        },
        new()
        {
            Title = "Attack on Titan",
            Author = "Hajime Isayama",
            Description = "Humanity's fight for survival against giant humanoid creatures.",
            Category = "Horror",
            VolumeCount = 34,
            Point = 9.8,
            ImageUrl = "https://example.com/attackontitan.jpg",
            DetailUrl = "https://example.com/attackontitan",
            CreatedDate = SeedDate
        },
        new()
        {
            Title = "My Hero Academia",
            Author = "Kohei Horikoshi",
            Description = "In a world where superpowers are the norm, a boy without them strives to be a hero.",
            Category = "Superhero",
            VolumeCount = 30,
            Point = 9.2,
            ImageUrl = "https://example.com/myheroacademia.jpg",
            DetailUrl = "https://example.com/myheroacademia",
            CreatedDate = SeedDate
        }
    ];
}
