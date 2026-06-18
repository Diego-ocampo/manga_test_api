using MangaT.ApplicationCore;
using MangaT.Domain.Entities;
using MangaT.Domain.Exceptions;
using MangaT.Domain.ValueObjects;

namespace MangaT.Tests.Unit.Domain;

/// <summary>Verifica las reglas del value object Rating (rango 0–10).</summary>
public class RatingTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    public void Should_Create_When_Value_Is_In_Range(double value)
    {
        var rating = new Rating(value);
        Assert.Equal(value, rating.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10.1)]
    public void Should_Throw_When_Value_Is_Out_Of_Range(double value)
    {
        Assert.Throws<DomainException>(() => new Rating(value));
    }
}

/// <summary>Verifica invariantes y métodos de la entidad de dominio Manga.</summary>
public class MangaDomainTests
{
    [Fact]
    public void Create_Should_Initialize_Manga_With_Valid_Data()
    {
        var manga = Manga.Create(
            "One Piece",
            "Eiichiro Oda",
            "A pirate adventure",
            "Adventure",
            100,
            new Rating(9.5),
            "https://example.com/image.jpg",
            "https://example.com/detail");

        Assert.Equal("One Piece", manga.Title);
        Assert.Equal(9.5, manga.Point);
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Is_Empty()
    {
        Assert.Throws<DomainException>(() => Manga.Create(
            "",
            "Author",
            "Description",
            "Category",
            1,
            new Rating(8),
            "https://example.com/image.jpg",
            "https://example.com/detail"));
    }

    [Fact]
    public void UpdateRating_Should_Change_Point()
    {
        var manga = Manga.Create(
            "Naruto",
            "Masashi Kishimoto",
            "Ninja story",
            "Action",
            72,
            new Rating(9),
            "https://example.com/image.jpg",
            "https://example.com/detail");

        manga.UpdateRating(new Rating(9.8));

        Assert.Equal(9.8, manga.Point);
    }
}
