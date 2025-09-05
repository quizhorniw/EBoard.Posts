using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Search;

public sealed record SearchPostsQuery(
    string? Title,
    Guid? CategoryId,
    Guid? UserId,
    decimal MinPrice,
    decimal MaxPrice,
    int Page = 1,
    int PageSize = 10
    ) : IRequest<IReadOnlyList<PostDto>>;