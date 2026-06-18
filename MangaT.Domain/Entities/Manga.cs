using MangaT.Domain.Exceptions;
using MangaT.Domain.ValueObjects;

namespace MangaT.Domain.Entities;

/// <summary>
/// Agregado raíz del dominio. Encapsula reglas de negocio e impide estados inválidos.
/// </summary>
public class Manga
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public int VolumeCount { get; private set; }
    public double Point { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public string DetailUrl { get; private set; } = string.Empty;
    public DateTime CreatedDate { get; private set; }

    /// <summary>Constructor privado requerido por EF Core.</summary>
    private Manga()
    {
    }

    /// <summary>Fábrica para crear un manga nuevo con validaciones de dominio.</summary>
    public static Manga Create(
        string title,
        string author,
        string description,
        string category,
        int volumeCount,
        Rating rating,
        string imageUrl,
        string detailUrl)
    {
        ValidateTitle(title);
        ValidateAuthor(author);
        ValidateVolumeCount(volumeCount);

        return new Manga
        {
            Title = title.Trim(),
            Author = author.Trim(),
            Description = description.Trim(),
            Category = category.Trim(),
            VolumeCount = volumeCount,
            Point = rating.Value,
            ImageUrl = imageUrl.Trim(),
            DetailUrl = detailUrl.Trim(),
            CreatedDate = DateTime.UtcNow
        };
    }

    /// <summary>Actualiza todos los campos editables del manga.</summary>
    public void Update(
        string title,
        string author,
        string description,
        string category,
        int volumeCount,
        Rating rating,
        string imageUrl,
        string detailUrl)
    {
        ValidateTitle(title);
        ValidateAuthor(author);
        ValidateVolumeCount(volumeCount);

        Title = title.Trim();
        Author = author.Trim();
        Description = description.Trim();
        Category = category.Trim();
        VolumeCount = volumeCount;
        Point = rating.Value;
        ImageUrl = imageUrl.Trim();
        DetailUrl = detailUrl.Trim();
    }

    /// <summary>Actualiza solo la calificación del manga.</summary>
    public void UpdateRating(Rating rating) => Point = rating.Value;

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("El título es obligatorio.");
        }
    }

    private static void ValidateAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new DomainException("El autor es obligatorio.");
        }
    }

    private static void ValidateVolumeCount(int volumeCount)
    {
        if (volumeCount < 0)
        {
            throw new DomainException("El número de volúmenes no puede ser negativo.");
        }
    }
}
