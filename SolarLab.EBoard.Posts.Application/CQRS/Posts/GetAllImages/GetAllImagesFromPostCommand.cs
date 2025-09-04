using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.GetAllImages;

public sealed record GetAllImagesFromPostCommand(Guid Id) : IRequest<List<ImageDto>>;