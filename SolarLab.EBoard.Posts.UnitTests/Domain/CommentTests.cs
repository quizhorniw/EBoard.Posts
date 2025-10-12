using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Domain;

public class CommentTests
{
    private static readonly Guid TestPostId = Guid.Parse("d2eb359d-9d6c-4f20-b22e-4a9fc91844d5"); 
    private static readonly Guid TestUserId = Guid.Parse("43bef27b-ca9d-490d-a998-62e7f06f3bf7"); 
    private const string TestText = "Text";
    private static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    [Theory]
    [InlineData("d2eb359d-9d6c-4f20-b22e-4a9fc91844d5", "43bef27b-ca9d-490d-a998-62e7f06f3bf7", "Comment 1")]
    [InlineData("8cde7e16-4362-474b-bb65-f7406b2d65b3", "0214c189-43bc-4f06-a5eb-33d7f98799e3", " some-comment-2")]
    [InlineData("afc495d3-fedf-4aaf-bb3c-dd624351d373", "2b8d608b-1650-4808-914c-f2b3ce34d3a5", "  Another Comment #3 ")]
    public void CreateComment_WithValidData_CreatesComment(string postId, string userId, string text)
    {
        // Arrange
        var postIdGuid = Guid.Parse(postId);
        var userIdGuid = Guid.Parse(userId);
        
        // Act
        var result = Comment.Create(postIdGuid, userIdGuid, text, TestDateTime);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(postIdGuid, result.PostId);
        Assert.Equal(userIdGuid, result.UserId);
        Assert.Equal(text.Trim(), result.Text);
        Assert.Equal(TestDateTime, result.CreatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void CreateComment_WithNullOrWhitespaceText_Throws(string text)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => Comment.Create(TestPostId, TestUserId, text, TestDateTime));
    }

    [Theory]
    [InlineData("title-1")]
    [InlineData("Another - title - 2")]
    [InlineData("  TiTlE 3 ")]
    public void ChangeCommentText_WithValidNewText_ChangesCommentText(string newText)
    {
        // Arrange
        var comment = Comment.Create(TestPostId, TestUserId, TestText, TestDateTime);
        
        // Act
        comment.ChangeText(newText);
        
        // Assert
        Assert.Equal(newText.Trim(), comment.Text);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void ChangeCommentText_WithNullOrWhitespaceNewText_Throws(string newText)
    {
        // Arrange
        var comment = Comment.Create(TestPostId, TestUserId, TestText, TestDateTime);
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => comment.ChangeText(newText));
    }
}