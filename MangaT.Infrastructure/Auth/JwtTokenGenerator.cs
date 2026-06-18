using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MangaT.Infrastructure.Auth;

/// <summary>
/// Genera JWT firmados con la clave de appsettings.
/// Los detalles de System.IdentityModel.Tokens.Jwt quedan en Infrastructure (capa externa).
/// </summary>
public sealed class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    /// <inheritdoc />
    public LoginResponse GenerateToken(AuthenticatedUser user)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key no configurada.");
        var issuer = jwtSettings["Issuer"] ?? "MangaT.API";
        var audience = jwtSettings["Audience"] ?? "MangaT.Client";
        var expirationMinutes = int.TryParse(jwtSettings["ExpirationMinutes"], out var minutes) ? minutes : 60;
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }
}
