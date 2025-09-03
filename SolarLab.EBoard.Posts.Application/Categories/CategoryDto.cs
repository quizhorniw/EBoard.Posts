namespace SolarLab.EBoard.Posts.Application.Categories;

public sealed record CategoryDto(Guid Id, string Name, Guid? ParentId);