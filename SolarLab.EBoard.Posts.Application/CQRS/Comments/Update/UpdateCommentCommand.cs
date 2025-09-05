using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.Update;

public sealed record UpdateCommentCommand(Guid Id, string Text) : IRequest;