using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.Create;

public sealed record CreateCommentCommand(Guid PostId, string Text) : IRequest<Guid>;