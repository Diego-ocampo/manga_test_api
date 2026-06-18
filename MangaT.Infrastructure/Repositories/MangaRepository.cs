using MangaT.ApplicationCore.Common;
using MangaT.ApplicationCore.Interfaces;
using MangaT.Domain.Entities;
using MangaT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MangaT.Infrastructure.Repositories;

public class MangaRepository(MangaDbContext context) : IMangaRepository
{
    public async Task<PagedResult<Manga>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = context.Mangas.OrderBy(m => m.Title);
        return await ToPagedResultAsync(query, page, pageSize, cancellationToken);
    }

    public async Task<PagedResult<Manga>> GetPopularAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = context.Mangas.OrderByDescending(m => m.Point);
        return await ToPagedResultAsync(query, page, pageSize, cancellationToken);
    }

    public async Task<Manga?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Mangas.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task AddAsync(Manga manga, CancellationToken cancellationToken = default)
    {
        await context.Mangas.AddAsync(manga, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Manga manga, CancellationToken cancellationToken = default)
    {
        context.Mangas.Update(manga);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Manga manga, CancellationToken cancellationToken = default)
    {
        context.Mangas.Remove(manga);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<PagedResult<Manga>> ToPagedResultAsync(
        IQueryable<Manga> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Manga>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
