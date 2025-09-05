using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.GetById;

public sealed class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostReadModel?>
{
    private readonly IPostsQueries _postsQueries;

    public GetPostByIdHandler(IPostsQueries postsQueries)
    {
        _postsQueries = postsQueries;
    }

    public async Task<PostReadModel?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        return await _postsQueries.GetByIdAsync(request.Id, cancellationToken);
    }
}