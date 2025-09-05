using SolarLab.EBoard.Posts.Application.CQRS.Posts;
using SolarLab.EBoard.Posts.Domain.Commons;

namespace SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

public interface IPostsQueries
{
    Task<PagedResult<PostReadModel>> SearchAsync(string? title,
        Guid? categoryId,
        Guid? userId,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<PostReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}