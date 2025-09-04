using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;

public sealed record GetCommentsByPostIdQuery(Guid PostId) : IRequest<IEnumerable<CommentDto>>;