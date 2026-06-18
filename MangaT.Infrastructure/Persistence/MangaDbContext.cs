using MangaT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaT.Infrastructure.Persistence;

/// <summary>
/// Contexto de Entity Framework Core para la base de datos de mangas.
/// </summary>
public class MangaDbContext : DbContext
{
    public MangaDbContext(DbContextOptions<MangaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Manga> Mangas { get; set; } = null!;

    /// <summary>Aplica configuraciones IEntityTypeConfiguration del ensamblado de infraestructura.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MangaDbContext).Assembly);
    }
}
