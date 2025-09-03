using MediatR;

namespace SolarLab.EBoard.Posts.Application.Comments.Create;

public sealed record CreateCommentCommand(Guid PostId, string Text) : IRequest<Guid>;