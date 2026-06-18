namespace MangaT.ApplicationCore.DTOs;

/// <summary>Representación pública de un manga devuelta por la API.</summary>
public class MangaDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public int VolumeCount { get; set; }
    public double Point { get; set; }
    public required string ImageUrl { get; set; }
    public required string DetailUrl { get; set; }
    public DateTime CreatedDate { get; set; }

    /// <summary>Campo calculado útil para listados y documentación OpenAPI.</summary>
    public string TitleWithAuthor => $"{Title} by {Author}";
}
