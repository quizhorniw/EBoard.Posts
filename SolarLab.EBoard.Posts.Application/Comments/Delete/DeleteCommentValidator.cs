using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.Comments.Delete;

internal sealed class DeleteCommentValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}