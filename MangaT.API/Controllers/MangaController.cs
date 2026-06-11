using Microsoft.AspNetCore.Mvc;
using MangaT.ApplicationCore.Interfaces;

namespace MangaT.API.Controllers;

[ApiController]
[Route("api/v1/manga")]
public class MangaController(IMangaService mangaService) : ControllerBase
{
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopular(CancellationToken cancellationToken)
    {
        var popularManga = await mangaService.GetPopularAsync(cancellationToken);
        return Ok(popularManga);
    }
}
