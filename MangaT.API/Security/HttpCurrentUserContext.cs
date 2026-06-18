using System.Security.Claims;
using MangaT.ApplicationCore.Interfaces;

namespace MangaT.API.Security;

/// <summary>
/// Adaptador HTTP → ICurrentUserContext (Dependency Inversion).
/// La capa API conoce HttpContext; ApplicationCore solo ve la abstracción.
/// </summary>
public sealed class HttpCurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    /// <inheritdoc />
    public string? Username =>
        httpContextAccessor.HttpContext?.User.Identity?.Name;

    /// <inheritdoc />
    public string? Role =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

    /// <inheritdoc />
    public bool IsInRole(string role) =>
        httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
}
