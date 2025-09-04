using MediatR;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.Create;

public sealed record CreateCategoryCommand(string Name, Guid? ParentId) : IRequest<Guid>;