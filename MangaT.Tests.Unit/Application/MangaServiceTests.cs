using FluentValidation;
using FluentValidation.Results;
using MangaT.ApplicationCore.DTOs;
using MangaT.ApplicationCore.Interfaces;
using MangaT.ApplicationCore.Services;
using MangaT.Domain.Entities;
using MangaT.Domain.Exceptions;
using MangaT.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace MangaT.Tests.Unit.Application;

/// <summary>
/// Pruebas unitarias de <see cref="MangaService"/> con repositorio y validadores simulados (Moq).
/// </summary>
public class MangaServiceTests
{
    private readonly Mock<IMangaRepository> _repository = new();
    private readonly Mock<ICurrentUserContext> _currentUser = new();
    private readonly Mock<IValidator<CreateMangaRequest>> _createValidator = new();
    private readonly Mock<IValidator<UpdateMangaRequest>> _updateValidator = new();
    private readonly Mock<ILogger<MangaService>> _logger = new();

    public MangaServiceTests()
    {
        _currentUser.Setup(u => u.IsInRole("Reader")).Returns(false);

        _createValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateMangaRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _updateValidator
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateMangaRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_MangaNotFoundException_When_Manga_Does_Not_Exist()
    {
        _repository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<IReadOnlyList<MangaT.Domain.Specifications.ISpecification<Manga>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Manga?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<MangaNotFoundException>(() => service.GetByIdAsync(99));
    }

    [Fact]
    public async Task CreateAsync_Should_Return_Dto_When_Request_Is_Valid()
    {
        Manga? savedManga = null;
        _repository.Setup(r => r.AddAsync(It.IsAny<Manga>(), It.IsAny<CancellationToken>()))
            .Callback<Manga, CancellationToken>((manga, _) => savedManga = manga)
            .Returns(Task.CompletedTask);

        var service = CreateService();
        var result = await service.CreateAsync(new CreateMangaRequest
        {
            Title = "Chainsaw Man",
            Author = "Tatsuki Fujimoto",
            Description = "Dark action manga",
            Category = "Action",
            VolumeCount = 15,
            Point = 9.1,
            ImageUrl = "https://example.com/csm.jpg",
            DetailUrl = "https://example.com/csm"
        });

        Assert.Equal("Chainsaw Man", result.Title);
        Assert.NotNull(savedManga);
        Assert.Equal("Chainsaw Man", savedManga!.Title);
    }

    private MangaService CreateService() =>
        new(
            _repository.Object,
            _currentUser.Object,
            _createValidator.Object,
            _updateValidator.Object,
            _logger.Object);
}
