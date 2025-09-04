using MediatR;

namespace SolarLab.EBoard.Posts.Application.Posts.GetAllImages;

public sealed record GetAllImagesFromPostCommand(Guid Id) : IRequest<List<ImageDto>>;