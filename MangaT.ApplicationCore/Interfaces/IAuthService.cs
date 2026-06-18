using MangaT.ApplicationCore.DTOs;

namespace MangaT.ApplicationCore.Interfaces;

/// <summary>Contrato de autenticación y emisión de tokens JWT.</summary>
public interface IAuthService
{
    /// <returns>Null si las credenciales no coinciden con ningún usuario demo.</returns>
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
