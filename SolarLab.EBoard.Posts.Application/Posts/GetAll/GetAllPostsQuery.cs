using MediatR;

namespace SolarLab.EBoard.Posts.Application.Posts.GetAll;

public sealed record GetAllPostsQuery : IRequest<IEnumerable<PostDto>>;