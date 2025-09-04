using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.GetAll;

public sealed record GetAllPostsQuery : IRequest<IEnumerable<PostDto>>;