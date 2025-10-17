namespace SolarLab.EBoard.Posts.Application.Abstractions.Users;

public interface IUsersService
{
    Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
}