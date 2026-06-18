using MangaT.Domain.Exceptions;

namespace MangaT.Domain.ValueObjects;

public sealed record Rating
{
    public double Value { get; }

    public Rating(double value)
    {
        if (value is < 0 or > 10)
        {
            throw new DomainException("La calificación debe estar entre 0 y 10.");
        }

        Value = value;
    }
}
