using MediatR;

namespace SolarLab.EBoard.Posts.Application.Categories.Update;

public sealed record UpdateCategoryCommand(Guid Id, string Name, Guid? ParentId) : IRequest;