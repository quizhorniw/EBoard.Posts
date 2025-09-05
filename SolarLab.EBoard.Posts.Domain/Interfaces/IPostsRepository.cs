using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Domain.Interfaces;

public interface IPostsRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    Task UpdateAsync(Post post, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}