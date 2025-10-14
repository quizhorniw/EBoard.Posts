using Microsoft.AspNetCore.Http;
using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.AddImages;
using SolarLab.EBoard.Posts.Domain.Entities;
using static SolarLab.EBoard.Posts.UnitTests.TestPostConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class AddImagesToPostCommandTests
{
    private readonly Mock<IPostsRepository> _postsRepositoryMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly AddImagesToPostHandler _handler;

    public AddImagesToPostCommandTests()
    {
        _postsRepositoryMock = new Mock<IPostsRepository>();
        _storageServiceMock = new Mock<IStorageService>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new AddImagesToPostHandler(
            _postsRepositoryMock.Object,
            _storageServiceMock.Object,
            _userContextMock.Object);
    }

    [Fact]
    public async Task AddImagesToPost_ExistingInDatabaseAndRequestedByOwner_AddsImagesToPost()
    {
        // Arrange
        var postId = Guid.Parse("db7d31df-ac09-4caf-a793-a46fc13587c8");
        var userId = Guid.Parse("e4d3b22c-69d7-4454-9c4b-fd960d6b9153");
        var post = new Post(
            userId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);

        var filename = "pic1.png";
        var file = new MockFormFile("name", filename);
        var images = new List<IFormFile> { file };

        var request = new AddImagesToPostCommand(postId, images);

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _userContextMock.Setup(c => c.UserId).Returns(userId);
        _storageServiceMock.Setup(s => s.SaveAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(filename);
        
        Post? capturedPost = null;
        _postsRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Callback<Post, CancellationToken>((p, _) => capturedPost = p)
            .Returns(Task.CompletedTask);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedPost);
        Assert.Equal(images.Count, capturedPost.Images.Count);
    }
    
    [Fact]
    public async Task AddImagesToPost_ExistingInDatabaseAndRequestedByOwner_UpdatesPostInDatabase()
    {
        // Arrange
        var postId = Guid.Parse("db7d31df-ac09-4caf-a793-a46fc13587c8");
        var userId = Guid.Parse("e4d3b22c-69d7-4454-9c4b-fd960d6b9153");
        var post = new Post(
            userId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);

        var filename = "pic1.png";
        var file = new MockFormFile("name", filename);
        var images = new List<IFormFile> { file };

        var request = new AddImagesToPostCommand(postId, images);

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _userContextMock.Setup(c => c.UserId).Returns(userId);
        _storageServiceMock.Setup(s => s.SaveAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(filename);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Post>(p => p.Images.Count == images.Count), 
            It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task AddImagesToPost_NotExistingInDatabase_Throws()
    {
        // Arrange
        var postId = Guid.Parse("db7d31df-ac09-4caf-a793-a46fc13587c8");

        var filename = "pic1.png";
        var file = new MockFormFile("name", filename);
        var images = new List<IFormFile> { file };

        var request = new AddImagesToPostCommand(postId, images);

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Post);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>_handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task AddImagesToPost_ExistingInDatabaseRequestingNotByOwner_Throws()
    {
        // Arrange
        var postId = Guid.Parse("db7d31df-ac09-4caf-a793-a46fc13587c8");
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);

        var filename = "pic1.png";
        var file = new MockFormFile("name", filename);
        var images = new List<IFormFile> { file };

        var request = new AddImagesToPostCommand(postId, images);

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _userContextMock.Setup(c => c.UserId).Returns(Guid.Parse("b4db0cba-d164-4e80-b036-67fce1775c2e"));
        
        // Act
        // Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>_handler.Handle(request, CancellationToken.None));
    }
}