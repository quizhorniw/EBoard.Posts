using MediatR;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Commons;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Search;

public sealed record SearchPostsQuery(
    string? Title,
    Guid? CategoryId,
    Guid? UserId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page = 1,
    int PageSize = 10
    ) : IRequest<PagedResult<PostReadModel>>;