using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Update;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Comments;

public class UpdateCommentCommandTests
{
    private readonly Mock<ICommentsRepository> _commentsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly UpdateCommentHandler _handler;

    private static readonly Guid TestPostId = Guid.Parse("ab5d4b82-dc94-4882-858b-c8c1c9d97824");
    private static readonly Guid TestUserId = Guid.Parse("81907b99-c40e-475b-b1f5-d533dc1f7164");
    private static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public UpdateCommentCommandTests()
    {
        _commentsRepositoryMock = new Mock<ICommentsRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new UpdateCommentHandler(_commentsRepositoryMock.Object, _userContextMock.Object);
    }

    [Theory]
    [InlineData("Some text 1")]
    [InlineData("Another text 2")]
    [InlineData("t-e-x-t 3")]
    public async Task UpdateCommentText_RequestedByOwner_UpdatesCommentText(string text)
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new UpdateCommentCommand(comment.Id, text);

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);
        
        Comment? capturedComment = null;
        _commentsRepositoryMock
            .Setup(r => r.UpdateTextAsync(comment, It.IsAny<CancellationToken>()))
            .Callback<Comment, CancellationToken>((c, _) => capturedComment = c)
            .Returns(Task.CompletedTask);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedComment);
        Assert.Equal(text, capturedComment.Text);
    }
    
    [Theory]
    [InlineData("Some text 1")]
    [InlineData("Another text 2")]
    [InlineData("t-e-x-t 3")]
    public async Task UpdateCommentText_RequestedByOwner_UpdatesCommentInDatabase(string text)
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new UpdateCommentCommand(comment.Id, text);

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsRepositoryMock.Verify(r => 
                r.UpdateTextAsync(It.Is<Comment>(c => 
                        comment.Id == c.Id &&
                        comment.UserId == c.UserId &&
                        comment.PostId == c.PostId &&
                        text == c.Text), 
                    It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task UpdateCommentText_NotExistingInDatabase_Throws()
    {
        // Arrange
        var commentId = Guid.Parse("866e3628-c042-483d-95a0-034c2d51aa8c");
        
        var request = new UpdateCommentCommand(commentId, "Test text");

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Comment);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Theory]
    [InlineData("Some text 1")]
    [InlineData("Another text 2")]
    [InlineData("t-e-x-t 3")]
    public async Task UpdateCommentText_RequestedByAdministrator_UpdatesCommentText(string text)
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new UpdateCommentCommand(comment.Id, text);

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(true);
        _userContextMock.Setup(c => c.UserId).Returns(Guid.Parse("a79f113a-c91b-4380-8d27-8942723f598c"));
        
        Comment? capturedComment = null;
        _commentsRepositoryMock
            .Setup(r => r.UpdateTextAsync(comment, It.IsAny<CancellationToken>()))
            .Callback<Comment, CancellationToken>((c, _) => capturedComment = c)
            .Returns(Task.CompletedTask);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedComment);
        Assert.Equal(text, capturedComment.Text);
    }
    
    [Theory]
    [InlineData("Some text 1")]
    [InlineData("Another text 2")]
    [InlineData("t-e-x-t 3")]
    public async Task UpdateCommentText_RequestedByAdministrator_UpdatesCommentInDatabase(
        string text)
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new UpdateCommentCommand(comment.Id, text);

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(true);
        _userContextMock.Setup(c => c.UserId).Returns(Guid.Parse("a79f113a-c91b-4380-8d27-8942723f598c"));
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsRepositoryMock.Verify(r => 
                r.UpdateTextAsync(It.Is<Comment>(c => 
                        comment.Id == c.Id &&
                        comment.UserId == c.UserId &&
                        comment.PostId == c.PostId &&
                        text == c.Text), 
                    It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task UpdateCommentText_RequestedNotByOwnerOrAdministrator_Throws()
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new UpdateCommentCommand(comment.Id, "New Text");

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        _userContextMock.Setup(c => c.UserId).Returns(Guid.Parse("a79f113a-c91b-4380-8d27-8942723f598c"));
        
        // Act
        // Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(request, CancellationToken.None));
    }
}