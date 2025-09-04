namespace SolarLab.EBoard.Posts.Application.CQRS.Posts;

public sealed record ImageDto(string Url, string MimeType, long Size);