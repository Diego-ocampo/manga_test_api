using MangaT.ApplicationCore.DTOs;

namespace MangaT.ApplicationCore.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
