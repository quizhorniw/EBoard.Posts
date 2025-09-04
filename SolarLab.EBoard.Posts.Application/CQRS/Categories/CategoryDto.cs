namespace SolarLab.EBoard.Posts.Application.CQRS.Categories;

public sealed record CategoryDto(Guid Id, string Name, Guid? ParentId);