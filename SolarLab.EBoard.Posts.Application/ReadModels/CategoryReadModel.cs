namespace SolarLab.EBoard.Posts.Application.ReadModels;

public sealed record CategoryReadModel(Guid Id, string Name, Guid? ParentId);