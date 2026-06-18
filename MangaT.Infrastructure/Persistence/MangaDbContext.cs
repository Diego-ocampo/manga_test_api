using MangaT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaT.Infrastructure.Persistence;

public class MangaDbContext : DbContext
{
    public MangaDbContext(DbContextOptions<MangaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Manga> Mangas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MangaDbContext).Assembly);
    }
}
