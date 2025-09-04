using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.AddImages;

internal sealed class AddImagesToPostValidator : AbstractValidator<AddImagesToPostCommand>
{
    public AddImagesToPostValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
        RuleFor(p => p.Files).NotEmpty();
    }
}