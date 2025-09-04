using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Comments.Delete;

public sealed record DeleteCommentCommand(Guid Id) : IRequest;