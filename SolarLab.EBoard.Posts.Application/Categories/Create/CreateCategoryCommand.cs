using MediatR;

namespace SolarLab.EBoard.Posts.Application.Categories.Create;

public sealed record CreateCategoryCommand(string Name, Guid? ParentId) : IRequest<Guid>;