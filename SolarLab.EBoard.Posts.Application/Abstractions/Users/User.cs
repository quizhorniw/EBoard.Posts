namespace SolarLab.EBoard.Posts.Application.Abstractions.Users;

public sealed record User(
    Guid Id,
    string Email,
    string? PhoneNumber,
    string FirstName,
    string LastName,
    string PasswordHash,
    string Role,
    string? confirmationToken,
    bool isConfirmed);