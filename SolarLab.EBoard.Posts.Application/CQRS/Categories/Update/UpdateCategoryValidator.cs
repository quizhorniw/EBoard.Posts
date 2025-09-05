using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.Update;

internal sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(70);
    }
}