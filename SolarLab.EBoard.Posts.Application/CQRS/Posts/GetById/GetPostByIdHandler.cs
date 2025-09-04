using MediatR;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.GetById;

public sealed class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostDto?>
{
    private readonly IPostsRepository _postsRepository;

    public GetPostByIdHandler(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public async Task<PostDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _postsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null) return null;
        
        return new PostDto(
            result.Id,
            result.Title,
            result.Description,
            result.CategoryId,
            result.Price,
            result.UserId,
            result.CreatedAt
            );
    }
}