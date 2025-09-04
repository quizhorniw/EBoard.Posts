using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Update;

public sealed record UpdatePostCommand(
    Guid Id,
    string Title,
    string? Description,
    Guid CategoryId,
    decimal Price
    ) : IRequest;