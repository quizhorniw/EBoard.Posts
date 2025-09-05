namespace SolarLab.EBoard.Posts.Application.ReadModels;

public sealed record PostReadModel(
    Guid Id,
    string Title,
    string? Description,
    Guid CategoryId,
    decimal Price,
    Guid UserId,
    DateTime CreatedAt
    );