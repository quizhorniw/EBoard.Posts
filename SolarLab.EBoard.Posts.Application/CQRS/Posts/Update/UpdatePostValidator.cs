using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Update;

internal sealed class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Title).NotEmpty().Length(3, 100);
        RuleFor(c => c.Description).MaximumLength(1000);
        RuleFor(c => c.CategoryId).NotEmpty();
        RuleFor(c => c.Price).GreaterThanOrEqualTo(0);
    }
}