using MangaT.ApplicationCore.Interfaces;
using MangaT.Infrastructure.Persistence;
using MangaT.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MangaT.Infrastructure;

/// <summary>
/// Registro de servicios de infraestructura: EF Core, SQL Server y repositorios.
/// </summary>
public static class DependencyInjection
{
    /// <summary>Configura DbContext con SQL Server y el repositorio de mangas.</summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<MangaDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IMangaRepository, MangaRepository>();

        return services;
    }
}
