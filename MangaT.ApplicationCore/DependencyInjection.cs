using FluentValidation;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Services;
using MangaT.ApplicationCore.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace MangaT.ApplicationCore;

/// <summary>
/// Registro de servicios de la capa de aplicación en el contenedor DI.
/// Sintaxis C# 14: bloque <c>extension(IServiceCollection services)</c>.
/// </summary>
public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>Registra servicios, validadores FluentValidation y casos de uso.</summary>
        public IServiceCollection AddApplication()
        {
            services.AddScoped<IMangaService, MangaService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IValidator<CreateMangaRequest>, CreateMangaRequestValidator>();
            services.AddScoped<IValidator<UpdateMangaRequest>, UpdateMangaRequestValidator>();

            return services;
        }
    }
}
