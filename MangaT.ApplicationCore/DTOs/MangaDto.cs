namespace MangaT.ApplicationCore.DTOs;

public class MangaDto
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public int VolumeCount { get; set; }
    public double Point { get; set; }
    public required Uri ImageUrl { get; set; }
    public required Uri DetailUrl { get; set; }
    public string TitleWithAuthor => $"{Title} by {Author}";
}
