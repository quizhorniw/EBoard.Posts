using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Update;
using SolarLab.EBoard.Posts.Domain.Entities;
using static SolarLab.EBoard.Posts.UnitTests.Application.Posts.TestPostConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class UpdatePostCommandTests
{
    private readonly Mock<IPostsRepository> _postsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly UpdatePostHandler _handler;
    
    public UpdatePostCommandTests()
    {
        _postsRepositoryMock = new Mock<IPostsRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new UpdatePostHandler(_postsRepositoryMock.Object, _userContextMock.Object);
    }

    [Theory]
    [InlineData("Title 1", "Description 1", "996b8bcc-d09e-4818-b19a-1c458903f041", 11.11)]
    [InlineData("Test title 2", "Some description 2", "5af5f83b-6edb-47a6-9f77-d9247f7ce852", 99.22)]
    [InlineData("Another title 3", null, "c0a85fd9-b86b-415b-957d-4cdb87fe5423", 30.00)]
    [InlineData("4-title", "DeScRiPtIoN | 4", "1a89f33f-a5ad-48b8-81fb-346d84e02ab9", 0.00)]
    [InlineData("t i t l e # 5", "d-e-s-c-r-i-p-t-i-o-n-5", "4bc1387a-14b1-4fb8-bf42-9a3e18522533", 10.01)]
    public async Task UpdatePost_OwnedByRequestingUser_UpdatesPostDetails(string title, string? description, string categoryId, decimal price)
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);

        var categoryIdGuid = Guid.Parse(categoryId);
        var request = new UpdatePostCommand(
            post.Id,
            title,
            description,
            categoryIdGuid,
            price);

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);
        
        Post? capturedPost = null;
        _postsRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Callback<Post, CancellationToken>((p, _) => capturedPost = p)
            .Returns(Task.CompletedTask);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedPost);
        Assert.Equal(title, capturedPost.Title);
        Assert.Equal(description, capturedPost.Description);
        Assert.Equal(categoryIdGuid, capturedPost.CategoryId);
        Assert.Equal(price, capturedPost.Price);
    }

    [Fact]
    public async Task UpdatePost_OwnedByRequestingUser_UpdatesPostInDatabase()
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);

        var updatedTitle = "Updated title";
        var updatedDescription = "Updated description";
        var updatedCategoryId = Guid.Parse("ea376ca4-62b8-480e-8400-06a52782fdec");
        var updatedPrice = 299.99m;
        var request = new UpdatePostCommand(
            post.Id,
            updatedTitle,
            updatedDescription,
            updatedCategoryId,
            updatedPrice);

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _userContextMock.Setup(c => c.IsInRole("Admin")).Returns(false);
        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsRepositoryMock.Verify(r => 
                r.UpdateAsync(It.Is<Post>(p =>
                    Guid.Empty != p.Id &&
                    TestUserId == p.UserId &&
                    updatedTitle == p.Title &&
                    updatedDescription == p.Description &&
                    updatedCategoryId == p.CategoryId &&
                    updatedPrice == p.Price),
                    It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}