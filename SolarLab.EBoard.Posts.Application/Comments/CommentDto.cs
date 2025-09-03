namespace SolarLab.EBoard.Posts.Application.Comments;

public sealed record CommentDto(Guid Id, Guid PostId, Guid UserId, string Text);