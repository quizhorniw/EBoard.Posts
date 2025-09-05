using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.Create;

internal sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
    }
}