using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.Posts.Create;

internal sealed class CreatePostValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostValidator()
    {
        RuleFor(c => c.Title).NotEmpty().Length(3, 100);
        RuleFor(c => c.Description).MaximumLength(1000);
        RuleFor(c => c.CategoryId).NotEmpty();
        RuleFor(c => c.Price).GreaterThanOrEqualTo(0);
    }
}