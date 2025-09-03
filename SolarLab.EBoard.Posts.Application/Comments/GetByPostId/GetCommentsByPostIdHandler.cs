using MediatR;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.Comments.GetByPostId;

public class GetCommentsByPostIdHandler : IRequestHandler<GetCommentsByPostIdQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentsRepository _commentsRepository;

    public GetCommentsByPostIdHandler(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _commentsRepository.GetByPostIdAsync(request.AdPostId, cancellationToken);
        return result.Select(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Text));
    }
}