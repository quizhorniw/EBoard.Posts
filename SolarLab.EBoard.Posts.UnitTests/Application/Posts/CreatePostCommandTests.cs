using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Time;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Create;
using SolarLab.EBoard.Posts.Domain.Entities;
using static SolarLab.EBoard.Posts.UnitTests.TestPostConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class CreatePostCommandTests
{
    private readonly Mock<IPostsRepository> _postsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly CreatePostHandler _handler;
    
    public CreatePostCommandTests()
    {
        _postsRepositoryMock = new Mock<IPostsRepository>();
        _userContextMock = new Mock<IUserContext>();
        _dateTimeProvider = new FakeDateTimeProvider(
            new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        _handler = new CreatePostHandler(
            _postsRepositoryMock.Object,
            _userContextMock.Object,
            _dateTimeProvider);
    }

    [Theory]
    [InlineData("Title 1", "Description 1", "996b8bcc-d09e-4818-b19a-1c458903f041", 11.11)]
    [InlineData("Test title 2", "Some description 2", "5af5f83b-6edb-47a6-9f77-d9247f7ce852", 99.22)]
    [InlineData("Another title 3", null, "c0a85fd9-b86b-415b-957d-4cdb87fe5423", 30.00)]
    [InlineData("4-title", "DeScRiPtIoN | 4", "1a89f33f-a5ad-48b8-81fb-346d84e02ab9", 0.00)]
    [InlineData("t i t l e # 5", "d-e-s-c-r-i-p-t-i-o-n-5", "4bc1387a-14b1-4fb8-bf42-9a3e18522533", 10.01)]
    public async Task CreatePost_SavesPostToDatabase(string title, string? description, string categoryId, decimal price)
    {
        // Arrange
        var categoryIdGuid = Guid.Parse(categoryId);
        var request = new CreatePostCommand(
            title,
            description,
            categoryIdGuid,
            price);

        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _postsRepositoryMock.Verify(r => 
                r.AddAsync(It.Is<Post>(p => 
                    Guid.Empty != p.Id &&
                    TestUserId == p.UserId &&
                    title == p.Title &&
                    description == p.Description &&
                    categoryIdGuid == p.CategoryId &&
                    price == p.Price &&
                    _dateTimeProvider.UtcNow == p.CreatedAt), 
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreatePost_ReturnsCreatedPostsId()
    {
        // Arrange
        var request = new CreatePostCommand(
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice);

        _userContextMock.Setup(c => c.UserId).Returns(TestUserId);

        Post? capturedPost = null;
        _postsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Callback<Post, CancellationToken>((p, _) => capturedPost = p)
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedPost);
        Assert.Equal(capturedPost.Id, result);
    }
}