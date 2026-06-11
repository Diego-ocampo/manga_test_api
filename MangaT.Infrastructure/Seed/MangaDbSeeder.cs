using MangaT.Infrastructure.Persistence;
using MangaT.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace MangaT.Infrastructure.Seed;

public static class MangaDbSeeder
{
    public static async Task SeedAsync(MangaDbContext context, CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Mangas.AnyAsync(cancellationToken))
        {
            return;
        }

        await context.Mangas.AddRangeAsync(MangaSeedData.GetMangas(), cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
