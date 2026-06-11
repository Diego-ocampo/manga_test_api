using MangaT.ApplicationCore.Entities;
using MangaT.ApplicationCore.Interfaces;
using MangaT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MangaT.Infrastructure.Repositories;

public class MangaRepository(MangaDbContext context) : IMangaRepository
{
    public async Task<IReadOnlyList<Manga>> GetPopularAsync(CancellationToken cancellationToken = default)
    {
        return await context.Mangas
            .OrderByDescending(m => m.Point)
            .ToListAsync(cancellationToken);
    }
}
