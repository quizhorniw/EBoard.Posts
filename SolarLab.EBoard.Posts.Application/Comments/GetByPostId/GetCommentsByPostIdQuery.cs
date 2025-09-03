using MediatR;

namespace SolarLab.EBoard.Posts.Application.Comments.GetByPostId;

public sealed record GetCommentsByPostIdQuery(Guid AdPostId) : IRequest<IEnumerable<CommentDto>>;