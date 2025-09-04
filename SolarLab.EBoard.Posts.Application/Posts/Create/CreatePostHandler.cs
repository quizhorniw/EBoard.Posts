using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.Posts.Create;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IPostsRepository _postsRepository;
    private readonly IUserContext _userContext;

    public CreatePostHandler(IPostsRepository postsRepository, IUserContext userContext)
    {
        _postsRepository = postsRepository;
        _userContext = userContext;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post(
            _userContext.UserId,
            request.Title,
            request.Description,
            request.CategoryId,
            request.Price
            );
        
        await _postsRepository.AddAsync(post, cancellationToken);
        return post.Id;
    }
}