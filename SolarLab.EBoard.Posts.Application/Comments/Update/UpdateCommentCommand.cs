using MediatR;

namespace SolarLab.EBoard.Posts.Application.Comments.Update;

public sealed record UpdateCommentCommand(Guid Id, string Text) : IRequest;