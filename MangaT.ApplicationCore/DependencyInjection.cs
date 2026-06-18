using FluentValidation;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Services;
using MangaT.ApplicationCore.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace MangaT.ApplicationCore;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMangaService, MangaService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IValidator<CreateMangaRequest>, CreateMangaRequestValidator>();
        services.AddScoped<IValidator<UpdateMangaRequest>, UpdateMangaRequestValidator>();

        return services;
    }
}
