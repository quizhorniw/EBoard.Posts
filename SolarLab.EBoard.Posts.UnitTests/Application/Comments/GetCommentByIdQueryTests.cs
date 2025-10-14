using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.GetById;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Comments;

public class GetCommentByIdQueryTests
{
    private readonly Mock<ICommentsQueries> _commentsQueriesMock;
    private readonly GetCommentByIdHandler _handler;

    private static readonly Guid TestPostId = Guid.Parse("ab5d4b82-dc94-4882-858b-c8c1c9d97824");
    private static readonly Guid TestUserId = Guid.Parse("81907b99-c40e-475b-b1f5-d533dc1f7164");
    private static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public GetCommentByIdQueryTests()
    {
        _commentsQueriesMock = new Mock<ICommentsQueries>();

        _handler = new GetCommentByIdHandler(_commentsQueriesMock.Object);
    }

    [Fact]
    public async Task GetCommentById_ReturnsFoundComment()
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);
        var commentReadModel = new CommentReadModel(
            comment.Id,
            comment.PostId,
            comment.UserId,
            comment.Text);
        
        var request = new GetCommentByIdQuery(comment.Id);

        _commentsQueriesMock
            .Setup(q => q.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(commentReadModel);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(commentReadModel, result);
    }
    
    [Fact]
    public async Task GetCommentById_QueriesCommentInDatabase()
    {
        // Arrange
        var comment = Comment.Create(
            TestPostId,
            TestUserId,
            "Test Text",
            TestDateTime);
        var commentReadModel = new CommentReadModel(
            comment.Id,
            comment.PostId,
            comment.UserId,
            comment.Text);
        
        var request = new GetCommentByIdQuery(comment.Id);

        _commentsQueriesMock
            .Setup(q => q.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(commentReadModel);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsQueriesMock.Verify(q => q.GetByIdAsync(comment.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task GetCommentById_NotExistingInDatabase_ReturnsNull()
    {
        // Arrange
        var commentId = Guid.Parse("e0d81082-ed60-4677-b776-c5440a548443");
        
        var request = new GetCommentByIdQuery(commentId);

        _commentsQueriesMock
            .Setup(q => q.GetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CommentReadModel);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetCommentById_NotExistingInDatabase_QueriesCommentInDatabase()
    {
        // Arrange
        var commentId = Guid.Parse("e0d81082-ed60-4677-b776-c5440a548443");
        
        var request = new GetCommentByIdQuery(commentId);

        _commentsQueriesMock
            .Setup(q => q.GetByIdAsync(commentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CommentReadModel);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsQueriesMock.Verify(q => q.GetByIdAsync(commentId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}