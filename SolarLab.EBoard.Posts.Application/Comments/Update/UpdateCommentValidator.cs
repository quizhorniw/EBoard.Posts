using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.Comments.Update;

internal sealed class UpdateCommentValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Text).NotEmpty().MaximumLength(1000);
    }
}