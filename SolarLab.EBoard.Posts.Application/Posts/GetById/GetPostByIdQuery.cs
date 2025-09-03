using MediatR;

namespace SolarLab.EBoard.Posts.Application.Posts.GetById;

public sealed record GetPostByIdQuery(Guid Id) : IRequest<PostDto?>;