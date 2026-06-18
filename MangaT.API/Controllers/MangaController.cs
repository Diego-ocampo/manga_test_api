using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;

namespace MangaT.API.Controllers;

[ApiController]
[Route("api/v1/manga")]
public class MangaController(IMangaService mangaService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mangaService.GetPagedAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpGet("popular")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPopular(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mangaService.GetPopularAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var manga = await mangaService.GetByIdAsync(id, cancellationToken);
        return Ok(manga);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateMangaRequest request, CancellationToken cancellationToken)
    {
        var manga = await mangaService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = manga.Id }, manga);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMangaRequest request, CancellationToken cancellationToken)
    {
        var manga = await mangaService.UpdateAsync(id, request, cancellationToken);
        return Ok(manga);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mangaService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
