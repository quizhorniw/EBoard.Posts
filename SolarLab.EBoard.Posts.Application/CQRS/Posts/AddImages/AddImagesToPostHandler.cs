using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;
using SolarLab.EBoard.Posts.Domain.Interfaces;
using SolarLab.EBoard.Posts.Domain.ValueObjects;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.AddImages;

public sealed class AddImagesToPostHandler : IRequestHandler<AddImagesToPostCommand>
{
    private readonly IPostsRepository _postsRepository;
    private readonly IStorageService _storageService;
    private readonly IUserContext _userContext;

    public AddImagesToPostHandler(IPostsRepository postsRepository, IStorageService storageService, IUserContext userContext)
    {
        _postsRepository = postsRepository;
        _storageService = storageService;
        _userContext = userContext;
    }

    public async Task Handle(AddImagesToPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (post is null)
        {
            throw new KeyNotFoundException("Post not found");
        }

        if (_userContext.UserId != post.UserId)
        {
            throw new UnauthorizedAccessException("No permission to update this ad post");
        }
        
        foreach (var file in request.Files)
        {
            var fileName = await _storageService.SaveAsync(file, cancellationToken);
            var image = new Image(fileName, file.ContentType, file.Length);
            post.AddImage(image);
        }
        
        await _postsRepository.UpdateAsync(post, cancellationToken);
    }
}