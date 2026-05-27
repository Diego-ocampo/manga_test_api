using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MangaT.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MangaController : ControllerBase
    {
        [HttpGet]
        [Route("api/v1/popular")]
        public IEnumerable<Manga> Get()
        {
            var popularManga = new List<Manga>         
           
            {
                new Manga
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
                new Manga
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
                new Manga
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
                new Manga
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
                new Manga
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

            return popularManga.OrderBy(m => m.Point);
        }


        public class Manga
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public int VolumeCount { get; set; }
            public double Point { get; set; }
            public Uri ImageUrl { get; set; }
            public Uri DetailUrl { get; set; }
        }
    }
}
