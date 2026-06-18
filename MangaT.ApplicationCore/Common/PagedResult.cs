namespace MangaT.ApplicationCore.Common;

/// <summary>Resultado paginado genérico con metadatos de navegación.</summary>
public class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }

    /// <summary>Total de páginas calculado a partir de TotalCount y PageSize.</summary>
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
