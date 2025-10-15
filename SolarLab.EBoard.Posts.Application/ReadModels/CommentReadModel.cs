namespace SolarLab.EBoard.Posts.Application.ReadModels;

public sealed record CommentReadModel(Guid Id, Guid PostId, Guid UserId, string Text);