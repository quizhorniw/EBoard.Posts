using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Delete;

public sealed class DeletePostHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IPostsRepository _postsRepository;
    private readonly IPostsQueries _postsQueries;
    private readonly IUserContext _userContext;
    
    public DeletePostHandler(IPostsRepository postsRepository, IUserContext userContext, IPostsQueries postsQueries)
    {
        _postsRepository = postsRepository;
        _userContext = userContext;
        _postsQueries = postsQueries;
    }

    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postsQueries.GetByIdAsync(request.Id, cancellationToken);
        if (post is null) return;
        
        if (!_userContext.IsInRole("Admin") && _userContext.UserId != post.UserId)
        {
            throw new UnauthorizedAccessException("No permission to delete this ad post");
        }
        
        await _postsRepository.DeleteAsync(request.Id, cancellationToken);
    }
}