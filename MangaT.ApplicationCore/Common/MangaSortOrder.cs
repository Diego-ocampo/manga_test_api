namespace MangaT.ApplicationCore.Common;

/// <summary>Ordenación soportada en consultas paginadas del repositorio.</summary>
public enum MangaSortOrder
{
    /// <summary>Alfabético por título (listado general).</summary>
    TitleAsc,

    /// <summary>Mayor calificación primero (endpoint /popular).</summary>
    PointDesc
}
