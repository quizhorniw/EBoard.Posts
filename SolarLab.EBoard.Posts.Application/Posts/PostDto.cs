namespace SolarLab.EBoard.Posts.Application.Posts;

public sealed record PostDto(
    Guid Id,
    string Title,
    string? Description,
    Guid CategoryId,
    decimal Price,
    Guid UserId,
    DateTime CreatedAt
    );