namespace MangaT.ApplicationCore.DTOs;

/// <summary>Payload para crear un manga (POST /api/v1/manga).</summary>
public class CreateMangaRequest
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int VolumeCount { get; set; }
    public double Point { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string DetailUrl { get; set; } = string.Empty;
}
