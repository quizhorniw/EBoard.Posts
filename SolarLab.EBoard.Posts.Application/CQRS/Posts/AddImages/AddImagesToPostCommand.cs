using MediatR;
using Microsoft.AspNetCore.Http;

namespace SolarLab.EBoard.Posts.Application.CQRS.Posts.AddImages;

public sealed record AddImagesToPostCommand(Guid Id, List<IFormFile> Files) : IRequest;