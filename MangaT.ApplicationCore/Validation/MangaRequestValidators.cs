using FluentValidation;
using MangaT.ApplicationCore.DTOs;

namespace MangaT.ApplicationCore.Validation;

public class CreateMangaRequestValidator : AbstractValidator<CreateMangaRequest>
{
    public CreateMangaRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(250);

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("El autor es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.Category)
            .MaximumLength(100);

        RuleFor(x => x.VolumeCount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Point)
            .InclusiveBetween(0, 10);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(1000);

        RuleFor(x => x.DetailUrl)
            .MaximumLength(1000);
    }
}

public class UpdateMangaRequestValidator : AbstractValidator<UpdateMangaRequest>
{
    public UpdateMangaRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(250);

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("El autor es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.Category)
            .MaximumLength(100);

        RuleFor(x => x.VolumeCount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Point)
            .InclusiveBetween(0, 10);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(1000);

        RuleFor(x => x.DetailUrl)
            .MaximumLength(1000);
    }
}
