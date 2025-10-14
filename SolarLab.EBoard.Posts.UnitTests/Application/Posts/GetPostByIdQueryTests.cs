using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.GetById;
using SolarLab.EBoard.Posts.Application.ReadModels;
using static SolarLab.EBoard.Posts.UnitTests.TestPostConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class GetPostByIdQueryTests
{
    private readonly Mock<IPostsQueries> _postsQueriesMock;
    private readonly GetPostByIdHandler _handler;
    
    public GetPostByIdQueryTests()
    {
        _postsQueriesMock = new Mock<IPostsQueries>();

        _handler = new GetPostByIdHandler(_postsQueriesMock.Object);
    }

    [Fact]
    public async Task GetPostById_ExistingInDatabase_ReturnsPostById()
    {
        // Arrange
        var postId = Guid.Parse("e4d9805d-769a-4e39-b566-5dce059ea592");
        var postReadModel = new PostReadModel(
            postId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestUserId,
            TestDateTime);
        
        var request = new GetPostByIdQuery(postId);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReadModel);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(postReadModel, result);
    }
    
    [Fact]
    public async Task GetPostById_ExistingInDatabase_QueriesPostInDatabase()
    {
        // Arrange
        var postId = Guid.Parse("e4d9805d-769a-4e39-b566-5dce059ea592");
        var postReadModel = new PostReadModel(
            postId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestUserId,
            TestDateTime);
        
        var request = new GetPostByIdQuery(postId);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReadModel);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsQueriesMock.Verify(q => q.GetByIdAsync(postId, It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task GetPostById_NotExistingInDatabase_ReturnsNull()
    {
        // Arrange
        var postId = Guid.Parse("e4d9805d-769a-4e39-b566-5dce059ea592");
        
        var request = new GetPostByIdQuery(postId);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as PostReadModel);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetPostById_NotExistingInDatabase_QueriesPostInDatabase()
    {
        // Arrange
        var postId = Guid.Parse("e4d9805d-769a-4e39-b566-5dce059ea592");
        
        var request = new GetPostByIdQuery(postId);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as PostReadModel);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsQueriesMock.Verify(q => q.GetByIdAsync(postId, It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}