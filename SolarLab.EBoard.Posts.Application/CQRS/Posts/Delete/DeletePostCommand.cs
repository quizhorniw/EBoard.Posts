using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Delete;

public sealed record DeletePostCommand(Guid Id) : IRequest;