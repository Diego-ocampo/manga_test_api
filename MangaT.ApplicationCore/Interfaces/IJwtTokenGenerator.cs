using MangaT.ApplicationCore.DTOs;

namespace MangaT.ApplicationCore.Interfaces;

/// <summary>
/// Genera tokens JWT (Dependency Inversion — SOLID "D").
/// Los detalles de System.IdentityModel.Tokens.Jwt viven en Infrastructure, no en ApplicationCore.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>Crea la respuesta de login con token firmado y fecha de expiración.</summary>
    LoginResponse GenerateToken(AuthenticatedUser user);
}
