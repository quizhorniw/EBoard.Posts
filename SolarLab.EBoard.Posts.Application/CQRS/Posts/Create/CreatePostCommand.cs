using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Create;

public sealed record CreatePostCommand(
    string Title,
    string? Description,
    Guid CategoryId,
    decimal Price
    ) : IRequest<Guid>;