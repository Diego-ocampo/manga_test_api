namespace MangaT.ApplicationCore.Entities;

public class Manga
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int VolumeCount { get; set; }
    public double Point { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string DetailUrl { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
