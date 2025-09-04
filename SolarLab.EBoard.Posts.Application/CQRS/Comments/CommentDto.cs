namespace SolarLab.EBoard.Posts.Application.CQRS.Comments;

public sealed record CommentDto(Guid Id, Guid PostId, Guid UserId, string Text);