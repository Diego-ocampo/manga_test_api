using MangaT.ApplicationCore.Common;
using MangaT.ApplicationCore.Interfaces;
using MangaT.Domain.Entities;
using MangaT.Domain.Specifications;
using MangaT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MangaT.Infrastructure.Repositories;

/// <summary>
/// Implementación de persistencia de mangas usando Entity Framework Core.
/// Aplica ISpecification traduciendo cada una a WHERE (Open/Closed en consultas).
/// </summary>
public class MangaRepository(MangaDbContext context) : IMangaRepository
{
    /// <inheritdoc />
    public Task<PagedResult<Manga>> QueryAsync(
        int page,
        int pageSize,
        IReadOnlyList<ISpecification<Manga>> specifications,
        MangaSortOrder sortOrder,
        CancellationToken cancellationToken = default)
    {
        var query = ApplySpecifications(context.Mangas, specifications);
        query = ApplySort(query, sortOrder);
        return ToPagedResultAsync(query, page, pageSize, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Manga?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Mangas.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Manga?> GetByIdAsync(
        int id,
        IReadOnlyList<ISpecification<Manga>> specifications,
        CancellationToken cancellationToken = default)
    {
        var query = ApplySpecifications(context.Mangas.Where(m => m.Id == id), specifications);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Manga manga, CancellationToken cancellationToken = default)
    {
        await context.Mangas.AddAsync(manga, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Manga manga, CancellationToken cancellationToken = default)
    {
        context.Mangas.Update(manga);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Manga manga, CancellationToken cancellationToken = default)
    {
        context.Mangas.Remove(manga);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Encadena WHERE por cada specification — extensible sin modificar este método.
    /// </summary>
    private static IQueryable<Manga> ApplySpecifications(
        IQueryable<Manga> query,
        IReadOnlyList<ISpecification<Manga>> specifications)
    {
        foreach (var specification in specifications)
        {
            query = query.Where(specification.ToExpression());
        }

        return query;
    }

    private static IQueryable<Manga> ApplySort(IQueryable<Manga> query, MangaSortOrder sortOrder) =>
        sortOrder switch
        {
            MangaSortOrder.PointDesc => query.OrderByDescending(m => m.Point),
            _ => query.OrderBy(m => m.Title)
        };

    /// <summary>Ejecuta conteo total y aplica Skip/Take para paginación eficiente.</summary>
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
