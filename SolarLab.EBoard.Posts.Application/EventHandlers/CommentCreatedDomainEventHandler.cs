using MediatR;
using Microsoft.Extensions.Logging;
using SolarLab.EBoard.Posts.Application.Abstractions.Messaging;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Users;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Application.EventHandlers;

public class CommentCreatedDomainEventHandler : INotificationHandler<CommentCreatedDomainEvent>
{
    private readonly ILogger<CommentCreatedDomainEventHandler> _logger;
    private readonly ICommentsRepository _commentsRepository;
    private readonly IPostsRepository _postsRepository;
    private readonly IUsersService _usersService;
    private readonly IMessageProducer _messageProducer;

    public CommentCreatedDomainEventHandler(
        ILogger<CommentCreatedDomainEventHandler> logger,
        ICommentsRepository commentsRepository,
        IPostsRepository postsRepository,
        IUsersService usersService,
        IMessageProducer messageProducer) 
    {
        _logger = logger;
        _commentsRepository = commentsRepository;
        _postsRepository = postsRepository;
        _usersService = usersService;
        _messageProducer = messageProducer;
    }

    public async Task Handle(CommentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var comment = await _commentsRepository.GetByIdAsync(notification.CommentId, cancellationToken);
        if (comment == null)
        {
            return;
        }

        var post = await _postsRepository.GetByIdAsync(comment.PostId, cancellationToken);
        if (post == null)
        {
            return;
        }

        var commentAuthor = await _usersService.GetUserAsync(comment.UserId, cancellationToken);
        if (commentAuthor == null)
        {
            return;
        }
        
        var postAuthor = await _usersService.GetUserAsync(post.UserId, cancellationToken);
        if (postAuthor == null)
        {
            return;
        }
        
        var message = new
        {
            to = new[] { postAuthor.Email },
            subject = "New comment on your post",
            content = $"""
                        <html>
                            <body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333;"">
                                <h2 style=""color: #4A90E2;"">New Comment on Your Post</h2>
                                <p>Hi {postAuthor.FirstName},</p>
                                <p>Your post <strong style=""color: #4A90E2;"">{post.Title}</strong> received a new comment:</p>
                                <blockquote style=""background-color: #f9f9f9; padding: 10px; border-left: 4px solid #ccc;"">
                                    <p><em>"{comment.Text}"</em></p>
                                    <small style=""color: #888;"">— {commentAuthor.FirstName} {commentAuthor.LastName} at {comment.CreatedAt:yyyy-MM-dd HH:mm}</small>
                                </blockquote>
                            </body>
                        </html>
                        """,
            isHtml = true
        };

        await _messageProducer.SendAsync(message, cancellationToken);
    }
}