using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

public interface ICommentsQueries
{
    Task<IReadOnlyList<CommentReadModel>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken = default);
    Task<CommentReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}