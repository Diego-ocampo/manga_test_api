using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace MangaT.ApplicationCore.Services;

/// <summary>
/// Caso de uso de login: orquesta validación de credenciales y emisión de token.
/// Single Responsibility + Dependency Inversion: no conoce appsettings ni JwtSecurityToken.
/// </summary>
public class AuthService(
    IUserCredentialValidator credentialValidator,
    IJwtTokenGenerator tokenGenerator,
    ILogger<AuthService> logger) : IAuthService
{
    /// <inheritdoc />
    public Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = credentialValidator.Validate(request.Username, request.Password);

        if (user is null)
        {
            logger.LogWarning(
                "Intento de login fallido para el usuario {Username}",
                request.Username);

            return Task.FromResult<LoginResponse?>(null);
        }

        var response = tokenGenerator.GenerateToken(user);

        logger.LogInformation(
            "Login exitoso para {Username} con rol {Role}",
            user.Username,
            user.Role);

        return Task.FromResult<LoginResponse?>(response);
    }
}
