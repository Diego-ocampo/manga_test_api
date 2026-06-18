using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;

namespace MangaT.API.Controllers;

/// <summary>
/// Endpoints CRUD de mangas.
/// Lectura: anónimo (catálogo completo) o Reader (solo Point &gt;= 7 vía specifications).
/// Escritura: solo Admin (política AdminOnly).
/// </summary>
[ApiController]
[Route("api/v1/manga")]
public class MangaController(IMangaService mangaService) : ControllerBase
{
    /// <summary>
    /// Lista mangas paginados por título.
    /// Filtro opcional: ?category=Action (Specification pattern — Open/Closed).
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? category = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mangaService.GetPagedAsync(page, pageSize, category, cancellationToken);
        return Ok(result);
    }

    /// <summary>Lista mangas ordenados por calificación (popular).</summary>
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

    /// <summary>Obtiene un manga por id. Reader no ve mangas con Point &lt; 7 (404).</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var manga = await mangaService.GetByIdAsync(id, cancellationToken);
        return Ok(manga);
    }

    /// <summary>Crea un manga. Requiere JWT Admin (política AdminOnly).</summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateMangaRequest request, CancellationToken cancellationToken)
    {
        var manga = await mangaService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = manga.Id }, manga);
    }

    /// <summary>Actualiza un manga. Requiere JWT Admin.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMangaRequest request, CancellationToken cancellationToken)
    {
        var manga = await mangaService.UpdateAsync(id, request, cancellationToken);
        return Ok(manga);
    }

    /// <summary>Elimina un manga. Reader recibe 403 Forbidden si intenta DELETE.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mangaService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
