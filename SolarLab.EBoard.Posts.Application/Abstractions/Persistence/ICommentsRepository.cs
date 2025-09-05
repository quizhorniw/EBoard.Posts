using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

public interface ICommentsRepository
{
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Task UpdateTextAsync(Comment comment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}