namespace SolarLab.EBoard.Posts.Application.ReadModels;

public record CommentReadModel(Guid Id, Guid PostId, Guid UserId, string Text);