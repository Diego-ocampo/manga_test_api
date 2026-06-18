namespace MangaT.ApplicationCore.DTOs;

/// <summary>Credenciales enviadas al endpoint de login.</summary>
public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

/// <summary>Token JWT y metadatos del usuario autenticado.</summary>
public class LoginResponse
{
    public required string Token { get; set; }
    public required string Username { get; set; }
    public required string Role { get; set; }
    public DateTime ExpiresAt { get; set; }
}
