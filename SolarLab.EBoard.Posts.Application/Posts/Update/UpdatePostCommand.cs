using MediatR;

namespace SolarLab.EBoard.Posts.Application.Posts.Update;

public sealed record UpdatePostCommand(
    Guid Id,
    string Title,
    string? Description,
    Guid CategoryId,
    decimal Price,
    string? ImagePath
    ) : IRequest;