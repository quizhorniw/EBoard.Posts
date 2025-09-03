using MediatR;

namespace SolarLab.EBoard.Posts.Application.Comments.GetByPostId;

public sealed record GetCommentsByPostIdQuery(Guid PostId) : IRequest<IEnumerable<CommentDto>>;