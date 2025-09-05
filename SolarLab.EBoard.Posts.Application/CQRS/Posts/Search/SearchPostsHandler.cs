using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Domain.Commons;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Search;

public sealed class SearchPostsHandler : IRequestHandler<SearchPostsQuery, PagedResult<PostReadModel>>
{
    private readonly IPostsQueries _postsQueries;

    public SearchPostsHandler(IPostsQueries postsQueries)
    {
        _postsQueries = postsQueries;
    }

    public async Task<PagedResult<PostReadModel>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
    {
        return await _postsQueries.SearchAsync(
            request.Title,
            request.CategoryId,
            request.UserId,
            request.MinPrice,
            request.MaxPrice,
            request.Page,
            request.PageSize,
            cancellationToken
            );
    }
}