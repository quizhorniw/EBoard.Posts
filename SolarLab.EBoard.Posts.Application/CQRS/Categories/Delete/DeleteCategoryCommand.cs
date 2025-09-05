using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest;