using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.Create;

public sealed class CreateCommentHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IUserContext _userContext;

    public CreateCommentHandler(ICommentsRepository commentsRepository, IUserContext userContext)
    {
        _commentsRepository = commentsRepository;
        _userContext = userContext;
    }

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = Comment.Create(request.PostId, _userContext.UserId, request.Text);
        await _commentsRepository.AddAsync(comment, cancellationToken);
        return comment.Id;
    }
}