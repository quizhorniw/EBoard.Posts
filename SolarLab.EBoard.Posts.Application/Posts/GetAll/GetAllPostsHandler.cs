using MediatR;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.Posts.GetAll;

public sealed class GetAllPostsHandler : IRequestHandler<GetAllPostsQuery, IEnumerable<PostDto>>
{
    private readonly IPostsRepository _postsRepository;

    public GetAllPostsHandler(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var result = await _postsRepository.GetAllAsync(cancellationToken);
        return result.Select(p => new PostDto(
            p.Id, 
            p.Title,
            p.Description,
            p.CategoryId,
            p.Price,
            p.UserId,
            p.CreatedAt
            ));
    }
}