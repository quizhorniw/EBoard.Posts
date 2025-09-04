using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.GetById;

public sealed record GetPostByIdQuery(Guid Id) : IRequest<PostDto?>;