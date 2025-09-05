using MediatR;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.GetById;

public sealed record GetPostByIdQuery(Guid Id) : IRequest<PostReadModel?>;