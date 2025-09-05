using SolarLab.EBoard.Posts.Domain.Commons;

namespace SolarLab.EBoard.Posts.Domain.Entities;

public class Comment : Entity
{
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private Comment(Guid postId, Guid userId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Comment cannot be empty", nameof(text));
        }

        Id = Guid.NewGuid();
        PostId = postId;
        UserId = userId;
        Text = text.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public static Comment Create(Guid postId, Guid userId, string text) => new(postId, userId, text);
    
    public void ChangeText(string newText) => Text = newText;
}