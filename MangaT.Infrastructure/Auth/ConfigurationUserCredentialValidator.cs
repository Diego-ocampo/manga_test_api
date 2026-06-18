using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MangaT.Infrastructure.Auth;

/// <summary>
/// Implementación concreta de validación de credenciales leyendo DemoUsers de configuración.
/// Dependency Inversion: ApplicationCore depende de IUserCredentialValidator, no de IConfiguration.
/// </summary>
public sealed class ConfigurationUserCredentialValidator(IConfiguration configuration) : IUserCredentialValidator
{
    /// <inheritdoc />
    public AuthenticatedUser? Validate(string username, string password)
    {
        var users = configuration.GetSection("DemoUsers").Get<List<DemoUser>>() ?? [];

        var user = users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);

        return user is null ? null : new AuthenticatedUser(user.Username, user.Role);
    }

    /// <summary>Modelo interno para deserializar DemoUsers desde appsettings.</summary>
    private sealed class DemoUser
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
