using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Delete;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Entities;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class DeletePostCommandTests
{
    private readonly Mock<IPostsRepository> _postsRepositoryMock;
    private readonly Mock<IPostsQueries> _postsQueriesMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly DeletePostHandler _handler;

    public DeletePostCommandTests()
    {
        _postsRepositoryMock = new Mock<IPostsRepository>();
        _postsQueriesMock = new Mock<IPostsQueries>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new DeletePostHandler(
            _postsRepositoryMock.Object,
            _userContextMock.Object,
            _postsQueriesMock.Object);
    }

    [Fact]
    public async Task DeletePost_OwnedByRequestingUser_DeletesPostInDatabase()
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);
        
        var request = new DeletePostCommand(post.Id);
        
        var postReadModel = new PostReadModel(
            post.Id,
            post.Title,
            post.Description,
            post.CategoryId,
            post.Price,
            post.UserId,
            post.CreatedAt);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReadModel);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsRepositoryMock.Verify(r =>
                r.DeleteAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletePost_NotExistingInDatabase_DoesNotDeletePostInDatabase()
    {
        // Arrange
        var postId = Guid.Parse("035f5cdd-71e4-4255-b322-98b48f129a90");
        var request = new DeletePostCommand(postId);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as PostReadModel);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsRepositoryMock.Verify(r =>
            r.DeleteAsync(request.Id, It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task DeletePost_ByAdministrator_DeletesPostInDatabase()
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);
        
        var request = new DeletePostCommand(post.Id);
        
        var postReadModel = new PostReadModel(
            post.Id,
            post.Title,
            post.Description,
            post.CategoryId,
            post.Price,
            post.UserId,
            post.CreatedAt);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReadModel);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(true);
        
        var differentUserGuid = Guid.Parse("f06eb5d0-42ea-47dd-8322-5dcb76938886");
        _userContextMock.Setup(c => c.UserId).Returns(differentUserGuid);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsRepositoryMock.Verify(r =>
            r.DeleteAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletePost_NotByOwnerOrAdministrator_Throws()
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);
        
        var request = new DeletePostCommand(post.Id);
        
        var postReadModel = new PostReadModel(
            post.Id,
            post.Title,
            post.Description,
            post.CategoryId,
            post.Price,
            post.UserId,
            post.CreatedAt);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReadModel);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        
        var differentUserGuid = Guid.Parse("f06eb5d0-42ea-47dd-8322-5dcb76938886");
        _userContextMock.Setup(c => c.UserId).Returns(differentUserGuid);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(request, CancellationToken.None));
    }
    
    [Fact]
    public async Task DeletePost_NotByOwnerOrAdministrator_DoesNotDeletePostInDatabase()
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);
        
        var request = new DeletePostCommand(post.Id);
        
        var postReadModel = new PostReadModel(
            post.Id,
            post.Title,
            post.Description,
            post.CategoryId,
            post.Price,
            post.UserId,
            post.CreatedAt);
        
        _postsQueriesMock
            .Setup(q => q.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postReadModel);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        
        var differentUserGuid = Guid.Parse("f06eb5d0-42ea-47dd-8322-5dcb76938886");
        _userContextMock.Setup(c => c.UserId).Returns(differentUserGuid);
        
        // Act
        try
        {
            await _handler.Handle(request, CancellationToken.None);
        }
        catch (UnauthorizedAccessException)
        {
        }
        
        // Assert
        _postsRepositoryMock.Verify(r =>
            r.DeleteAsync(request.Id, It.IsAny<CancellationToken>()), Times.Never);
    }
}