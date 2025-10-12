using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Domain;

public class CommentTests
{
    private readonly DateTime _testDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
        var result = Comment.Create(postIdGuid, userIdGuid, text, _testDateTime);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(postIdGuid, result.PostId);
        Assert.Equal(userIdGuid, result.UserId);
        Assert.Equal(text.Trim(), result.Text);
        Assert.Equal(_testDateTime, result.CreatedAt);
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
        var postId = Guid.Parse("8cde7e16-4362-474b-bb65-f7406b2d65b3");
        var userId = Guid.Parse("43bef27b-ca9d-490d-a998-62e7f06f3bf7");
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => Comment.Create(postId, userId, text, _testDateTime));
    }
    
}