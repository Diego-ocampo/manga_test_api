using Microsoft.AspNetCore.Mvc;
using MangaT.API.Models;
using System.Linq;

namespace MangaT.API.Controllers;

[ApiController]
[Route("api/v1/manga")]
public class MangaController : ControllerBase
{
    [HttpGet("popular")]
    public ActionResult<IEnumerable<Manga>> GetPopular()
    {
        var popularManga = new List<Manga>
           
        {
            new()
            {
                Title = "One Piece",
                Author = "Eiichiro Oda",
                Description = "A story about a group of pirates searching for the ultimate treasure.",
                Category = "Adventure",
                VolumeCount = 100,
                Point = 9.5,
                ImageUrl = new Uri("https://example.com/onepiece.jpg"),
                DetailUrl = new Uri("https://example.com/onepiece")
            },
            new()
            {
                Title = "Naruto",
                Author = "Masashi Kishimoto",
                Description = "A young ninja's journey to become the strongest in his village.",
                Category = "Action",
                VolumeCount = 72,
                Point = 9.0,
                ImageUrl = new Uri("https://example.com/naruto.jpg"),
                DetailUrl = new Uri("https://example.com/naruto")
            },
            new()
            {
                Title = "Bleach",
                Author = "Tite Kubo",
                Description = "A teenager gains the powers of a Soul Reaper and battles evil spirits.",
                Category = "Fantasy",
                VolumeCount = 74,
                Point = 8.5,
                ImageUrl = new Uri("https://example.com/bleach.jpg"),
                DetailUrl = new Uri("https://example.com/bleach")
            },
            new()
            {
                Title = "Attack on Titan",
                Author = "Hajime Isayama",
                Description = "Humanity's fight for survival against giant humanoid creatures.",
                Category = "Horror",
                VolumeCount = 34,
                Point = 9.8,
                ImageUrl = new Uri("https://example.com/attackontitan.jpg"),
                DetailUrl = new Uri("https://example.com/attackontitan")
            },
            new()
            {
                Title = "My Hero Academia",
                Author = "Kohei Horikoshi",
                Description = "In a world where superpowers are the norm, a boy without them strives to be a hero.",
                Category = "Superhero",
                VolumeCount = 30,
                Point = 9.2,
                ImageUrl = new Uri("https://example.com/myheroacademia.jpg"),
                DetailUrl = new Uri("https://example.com/myheroacademia")
            }
        };
        
        var list = popularManga.OrderByDescending(m => m.Point).ToList();
        return Ok(list);
    }
}
