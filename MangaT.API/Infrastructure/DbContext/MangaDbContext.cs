using Microsoft.EntityFrameworkCore;
using MangaT.API.Infrastructure.Entities;

namespace MangaT.API.Infrastructure.Persistence
{
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

            var manga = modelBuilder.Entity<Manga>();

            manga.HasKey(m => m.Id);

            manga.Property(m => m.Title)
                 .IsRequired()
                 .HasMaxLength(250);

            manga.Property(m => m.Author)
                 .IsRequired()
                 .HasMaxLength(150);

            manga.Property(m => m.Description)
                 .HasMaxLength(2000);

            manga.Property(m => m.Category)
                 .HasMaxLength(100);

            manga.Property(m => m.ImageUrl)
                 .HasMaxLength(1000);

            manga.Property(m => m.DetailUrl)
                 .HasMaxLength(1000);

            manga.Property(m => m.CreatedDate)
                 .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}