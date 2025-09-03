using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.Comments.Create;

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
        var comment = Comment.Create(request.AdPostId, _userContext.UserId, request.Text);
        await _commentsRepository.AddAsync(comment, cancellationToken);
        return comment.Id;
    }
}