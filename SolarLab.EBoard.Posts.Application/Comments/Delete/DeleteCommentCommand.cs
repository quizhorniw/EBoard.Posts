using MediatR;

namespace SolarLab.EBoard.Posts.Application.Comments.Delete;

public sealed record DeleteCommentCommand(Guid Id) : IRequest;