using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.Delete;

public sealed class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IUserContext _userContext;

    public DeleteCommentHandler(ICommentsRepository commentsRepository, IUserContext userContext)
    {
        _commentsRepository = commentsRepository;
        _userContext = userContext;
    }

    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (comment is null) return;
        
        if (!_userContext.IsInRole("Admin") && _userContext.UserId != comment.UserId)
        {
            throw new UnauthorizedAccessException("No permission to delete this comment");
        }

        await _commentsRepository.DeleteAsync(request.Id, cancellationToken);
    }
}