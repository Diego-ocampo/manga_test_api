using MangaT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mapeo de la entidad Manga a SQL Server: columnas, restricciones y datos semilla.
/// </summary>
public class MangaConfiguration : IEntityTypeConfiguration<Manga>
{
    // Fecha fija para datos semilla (HasData requiere valores constantes en migraciones).
    private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Manga> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(m => m.Author)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(m => m.Description)
               .HasMaxLength(2000);

        builder.Property(m => m.Category)
               .HasMaxLength(100);

        builder.Property(m => m.ImageUrl)
               .HasMaxLength(1000);

        builder.Property(m => m.DetailUrl)
               .HasMaxLength(1000);

        builder.Property(m => m.CreatedDate)
               .HasDefaultValueSql("GETUTCDATE()");

        // Catálogo inicial de mangas insertado por migración.
        builder.HasData(
            new
            {
                Id = 1,
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
            new
            {
                Id = 2,
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
            new
            {
                Id = 3,
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
            new
            {
                Id = 4,
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
            new
            {
                Id = 5,
                Title = "My Hero Academia",
                Author = "Kohei Horikoshi",
                Description = "In a world where superpowers are the norm, a boy without them strives to be a hero.",
                Category = "Superhero",
                VolumeCount = 30,
                Point = 9.2,
                ImageUrl = "https://example.com/myheroacademia.jpg",
                DetailUrl = "https://example.com/myheroacademia",
                CreatedDate = SeedDate
            });
    }
}
