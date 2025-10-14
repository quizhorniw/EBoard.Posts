using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;
using SolarLab.EBoard.Posts.Application.ReadModels;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Comments;

public class GetCommentByPostIdQueryTests
{
    private readonly Mock<ICommentsQueries> _commentsQueriesMock;
    private readonly GetCommentsByPostIdHandler _handler;
    
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