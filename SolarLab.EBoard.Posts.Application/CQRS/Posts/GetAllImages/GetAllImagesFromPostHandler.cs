using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.GetAllImages;

public sealed class GetAllImagesFromPostHandler : IRequestHandler<GetAllImagesFromPostCommand, List<ImageDto>>
{
    private readonly IPostsRepository _postsRepository;
    private readonly IUrlProvider _urlProvider;

    public GetAllImagesFromPostHandler(IPostsRepository postsRepository, IUrlProvider urlProvider)
    {
        _postsRepository = postsRepository;
        _urlProvider = urlProvider;
    }

    public async Task<List<ImageDto>> Handle(GetAllImagesFromPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postsRepository.GetByIdAsync(request.Id, cancellationToken);
        if (post is null)
        {
            throw new KeyNotFoundException("Post not found");
        }

        return post.Images
            .Select(i => new ImageDto(
                _urlProvider.GetUrl(i.FileName),
                i.MimeType,
                i.Size
                ))
            .ToList();
    }
}