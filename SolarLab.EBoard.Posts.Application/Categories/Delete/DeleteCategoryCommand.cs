using MediatR;

namespace SolarLab.EBoard.Posts.Application.Categories.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest;