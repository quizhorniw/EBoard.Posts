using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.GetById;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Comments;

public class GetCommentByPostIdQueryTests
{
    private readonly Mock<ICommentsQueries> _commentsQueriesMock;
    private readonly GetCommentsByPostIdHandler _handler;

    private static readonly Guid TestId = Guid.Parse("9ff9ef60-4b4c-47cd-b4c5-56b7408779b4");
    private static readonly Guid TestPostId = Guid.Parse("ab5d4b82-dc94-4882-858b-c8c1c9d97824");
    private static readonly Guid TestUserId = Guid.Parse("81907b99-c40e-475b-b1f5-d533dc1f7164");
    private const string TestText = "Test Text";
    private static readonly DateTime TestDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public GetCommentByPostIdQueryTests()
    {
        _commentsQueriesMock = new Mock<ICommentsQueries>();

        _handler = new GetCommentsByPostIdHandler(_commentsQueriesMock.Object);
    }

    [Fact]
    public async Task GetCommentsByPostId_ReturnsFoundComments()
    {
        // Arrange
        var commentReadModels = new List<CommentReadModel>
        {
            new(
                TestId,
                TestPostId,
                TestUserId,
                TestText)
        };
        
        var request = new GetCommentsByPostIdQuery(TestPostId);

        _commentsQueriesMock
            .Setup(q => q.GetByPostIdAsync(TestPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(commentReadModels);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(commentReadModels, result);
    }
    
    [Fact]
    public async Task GetCommentsByPostId_QueriesCommentsInDatabase()
    {
        // Arrange
        var commentReadModels = new List<CommentReadModel>
        {
            new(
                TestId,
                TestPostId,
                TestUserId,
                TestText)
        };
        
        var request = new GetCommentsByPostIdQuery(TestPostId);

        _commentsQueriesMock
            .Setup(q => q.GetByPostIdAsync(TestPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(commentReadModels);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsQueriesMock.Verify(q => q.GetByPostIdAsync(TestPostId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task GetCommentsByPostId_NotExistingInDatabase_ReturnsNoComments()
    {
        // Arrange
        var request = new GetCommentsByPostIdQuery(TestPostId);

        _commentsQueriesMock
            .Setup(q => q.GetByPostIdAsync(TestPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CommentReadModel>());
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetCommentsByPostId_NotExistingInDatabase_QueriesCommentsInDatabase()
    {
        // Arrange
        var request = new GetCommentsByPostIdQuery(TestPostId);

        _commentsQueriesMock
            .Setup(q => q.GetByPostIdAsync(TestPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CommentReadModel>());
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsQueriesMock.Verify(q => q.GetByPostIdAsync(TestPostId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}