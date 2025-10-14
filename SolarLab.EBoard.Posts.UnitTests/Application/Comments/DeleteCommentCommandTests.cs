using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Delete;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Comments;

public class DeleteCommentCommandTests
{
    private readonly Mock<ICommentsRepository> _commentsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly DeleteCommentHandler _handler;

    private static readonly Guid TestPostId = Guid.Parse("ab5d4b82-dc94-4882-858b-c8c1c9d97824");
    private static readonly Guid TestUserId = Guid.Parse("81907b99-c40e-475b-b1f5-d533dc1f7164");
    private static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public DeleteCommentCommandTests()
    {
        _commentsRepositoryMock = new Mock<ICommentsRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new DeleteCommentHandler(_commentsRepositoryMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task DeleteComment_RequestedByOwner_DeletesCommentInDatabase()
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new DeleteCommentCommand(comment.Id);

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsRepositoryMock.Verify(r => 
                r.DeleteAsync(comment.Id, It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task DeleteComment_NotExistingInDatabase_DoesNotDeleteCommentInDatabase()
    {
        // Arrange
        var commentId = Guid.Parse("ab04e978-1cf5-442b-8718-99d124dabea5");
            
        var request = new DeleteCommentCommand(commentId);

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Comment);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsRepositoryMock.Verify(r => 
                r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }
    
    [Fact]
    public async Task DeleteComment_RequestedByAdministrator_DeletesCommentInDatabase()
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new DeleteCommentCommand(comment.Id);

        _commentsRepositoryMock
            .Setup(r => r.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(true);
        _userContextMock.Setup(c => c.UserId).Returns(Guid.Parse("a79f113a-c91b-4380-8d27-8942723f598c"));
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsRepositoryMock.Verify(r => 
                r.DeleteAsync(comment.Id, It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task DeleteComment_RequestedNotByOwnerOrAdministrator_Throws()
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);

        var request = new DeleteCommentCommand(comment.Id);

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