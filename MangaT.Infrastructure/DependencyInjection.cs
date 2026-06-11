using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Services;
using MangaT.Infrastructure.Persistence;
using MangaT.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MangaT.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<MangaDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IMangaRepository, MangaRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMangaService, MangaService>();
        return services;
    }
}
