using MediatR;

namespace SolarLab.EBoard.Posts.Application.Posts.Delete;

public sealed record DeletePostCommand(Guid Id) : IRequest;