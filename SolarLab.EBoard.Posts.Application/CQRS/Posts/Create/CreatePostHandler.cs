using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Time;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.Create;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IPostsRepository _postsRepository;
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreatePostHandler(IPostsRepository postsRepository, IUserContext userContext,
        IDateTimeProvider dateTimeProvider)
    {
        _postsRepository = postsRepository;
        _userContext = userContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post(
            _userContext.UserId,
            request.Title,
            request.Description,
            request.CategoryId,
            request.Price,
            _dateTimeProvider.UtcNow
            );
        
        await _postsRepository.AddAsync(post, cancellationToken);
        return post.Id;
    }
}