using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.Delete;

internal sealed class DeleteCommentValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}