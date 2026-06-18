using FluentValidation;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Services;
using MangaT.ApplicationCore.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace MangaT.ApplicationCore;

/// <summary>
/// Registro de servicios de la capa de aplicación en el contenedor DI.
/// </summary>
public static class DependencyInjection
{
    /// <summary>Registra servicios, validadores FluentValidation y casos de uso.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMangaService, MangaService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IValidator<CreateMangaRequest>, CreateMangaRequestValidator>();
        services.AddScoped<IValidator<UpdateMangaRequest>, UpdateMangaRequestValidator>();

        return services;
    }
}
