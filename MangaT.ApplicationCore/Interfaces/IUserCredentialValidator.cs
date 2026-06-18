namespace MangaT.ApplicationCore.Interfaces;

/// <summary>
/// Valida credenciales de login (Dependency Inversion — SOLID "D").
/// La aplicación no sabe si los usuarios vienen de appsettings, LDAP o una BD.
/// </summary>
public interface IUserCredentialValidator
{
    /// <summary>
    /// Devuelve el usuario autenticado o null si las credenciales son inválidas.
    /// </summary>
    AuthenticatedUser? Validate(string username, string password);
}

/// <summary>Resultado de una validación exitosa (sin contraseña en memoria).</summary>
public sealed record AuthenticatedUser(string Username, string Role);
