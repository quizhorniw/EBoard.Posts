using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.Posts.Update;

public sealed class UpdatePostHandler : IRequestHandler<UpdatePostCommand>
{
    private readonly IPostsRepository _postsRepository;
    private readonly IUserContext _userContext;

    public UpdatePostHandler(IPostsRepository postsRepository, IUserContext userContext)
    {
        _postsRepository = postsRepository;
        _userContext = userContext;
    }

    public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (post is null)
        {
            throw new KeyNotFoundException("Ad post not found");
        }

        if (!_userContext.IsInRole("Admin") && _userContext.UserId != post.UserId)
        {
            throw new UnauthorizedAccessException("No permission to update this ad post");
        }
        
        post.UpdateDetails(
            request.Title,
            request.Description,
            request.CategoryId,
            request.Price
            );
        
        await _postsRepository.UpdateAsync(post, cancellationToken);
    }
}