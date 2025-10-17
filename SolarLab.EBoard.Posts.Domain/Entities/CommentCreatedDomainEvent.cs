using MediatR;

namespace SolarLab.EBoard.Posts.Domain.Entities;

public sealed record CommentCreatedDomainEvent(Guid CommentId) : INotification;