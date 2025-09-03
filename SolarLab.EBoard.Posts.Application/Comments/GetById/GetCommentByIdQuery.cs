using MediatR;

namespace SolarLab.EBoard.Posts.Application.Comments.GetById;

public sealed record GetCommentByIdQuery(Guid Id) : IRequest<CommentDto?>;