using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.Categories.Delete;

internal sealed class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}