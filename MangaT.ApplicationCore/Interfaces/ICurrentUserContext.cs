namespace MangaT.ApplicationCore.Interfaces;

/// <summary>
/// Abstracción del usuario actual (Dependency Inversion).
/// La capa de aplicación NO referencia HttpContext; la API implementa esta interfaz.
/// </summary>
public interface ICurrentUserContext
{
    /// <summary>Nombre del usuario autenticado, o null si es anónimo.</summary>
    string? Username { get; }

    /// <summary>Rol principal del JWT (Admin, Reader), o null si es anónimo.</summary>
    string? Role { get; }

    /// <summary>Indica si el usuario tiene el rol indicado (comparación case-insensitive).</summary>
    bool IsInRole(string role);
}
