using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetById;

public sealed class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentReadModel?>
{
    private readonly ICommentsQueries _commentsQueries;

    public GetCommentByIdHandler(ICommentsQueries commentsQueries)
    {
        _commentsQueries = commentsQueries;
    }

    public async Task<CommentReadModel?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _commentsQueries.GetByIdAsync(request.Id, cancellationToken);
    }
}