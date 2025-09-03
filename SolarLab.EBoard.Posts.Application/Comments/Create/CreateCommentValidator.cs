using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.Comments.Create;

internal sealed class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentValidator()
    {
        RuleFor(c => c.PostId).NotEmpty();
        RuleFor(c => c.Text).NotEmpty().MaximumLength(1000);
    }
}