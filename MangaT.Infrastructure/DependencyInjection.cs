using MangaT.ApplicationCore.Interfaces;
using MangaT.Infrastructure.Auth;
using MangaT.Infrastructure.Persistence;
using MangaT.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MangaT.Infrastructure;

/// <summary>
/// Registro de servicios de infraestructura: EF Core, SQL Server, auth y repositorios.
/// Sintaxis C# 14: bloques <c>extension</c> sobre <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>Implementaciones concretas de auth (JWT + usuarios demo).</summary>
        public IServiceCollection AddInfrastructureAuth()
        {
            services.AddScoped<IUserCredentialValidator, ConfigurationUserCredentialValidator>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }

        /// <summary>Configura DbContext con SQL Server, auth y el repositorio de mangas.</summary>
        public IServiceCollection AddInfrastructure(string connectionString)
        {
            services.AddDbContext<MangaDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddInfrastructureAuth();
            services.AddScoped<IMangaRepository, MangaRepository>();

            return services;
        }
    }
}
