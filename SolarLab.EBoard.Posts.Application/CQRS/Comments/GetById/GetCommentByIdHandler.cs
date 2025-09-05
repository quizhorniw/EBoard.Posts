using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetById;

public sealed class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentDto?>
{
    private readonly ICommentsRepository _commentsRepository;

    public GetCommentByIdHandler(ICommentsRepository commentsRepository)
    {
        _commentsRepository = commentsRepository;
    }

    public async Task<CommentDto?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _commentsRepository.GetByIdAsync(request.Id, cancellationToken);
        return result is null ? null : new CommentDto(result.Id, result.PostId, result.UserId, result.Text);
    }
}