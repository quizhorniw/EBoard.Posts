using MediatR;

namespace SolarLab.EBoard.Posts.Application.Posts.Create;

public sealed record CreatePostCommand(
    string Title,
    string? Description,
    Guid CategoryId,
    decimal Price,
    string? ImagePath
    ) : IRequest<Guid>;