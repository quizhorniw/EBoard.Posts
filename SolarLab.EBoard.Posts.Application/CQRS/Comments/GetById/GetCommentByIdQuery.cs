using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.GetById;

public sealed record GetCommentByIdQuery(Guid Id) : IRequest<CommentDto?>;