using System.Collections.Immutable;
using MediatR;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Search;

public sealed class SearchPostsHandler : IRequestHandler<SearchPostsQuery, IReadOnlyList<PostDto>>
{
    private readonly IPostsRepository _postsRepository;

    public SearchPostsHandler(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public async Task<IReadOnlyList<PostDto>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
    {
        var result = await _postsRepository.SearchAsync(
            request.Title,
            request.CategoryId,
            request.UserId,
            request.MinPrice,
            request.MaxPrice,
            request.Page,
            request.PageSize,
            cancellationToken
            );

        return result.Select(p => new PostDto(
            p.Id,
            p.Title,
            p.Description, 
            p.CategoryId,
            p.Price,
            p.UserId,
            p.CreatedAt
            )).ToImmutableList();
    }
}