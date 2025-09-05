using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;

public class GetCommentsByPostIdHandler : IRequestHandler<GetCommentsByPostIdQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentsRepository _commentsRepository;

    public GetCommentsByPostIdHandler(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _commentsRepository.GetByPostIdAsync(request.PostId, cancellationToken);
        return result.Select(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Text));
    }
}