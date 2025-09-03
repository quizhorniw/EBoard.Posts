using FluentValidation;

namespace SolarLab.EBoard.Posts.Application.Posts.Delete;

internal sealed class DeletePostValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}