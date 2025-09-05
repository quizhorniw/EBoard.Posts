using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;

public class GetCommentsByPostIdHandler : IRequestHandler<GetCommentsByPostIdQuery, IEnumerable<CommentReadModel>>
{
    private readonly ICommentsQueries _commentsQueries;

    public GetCommentsByPostIdHandler(ICommentsQueries commentsQueries)
    {
        _commentsQueries = commentsQueries;
    }

    public async Task<IEnumerable<CommentReadModel>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
    {
        return await _commentsQueries.GetByPostIdAsync(request.PostId, cancellationToken);
    }
}