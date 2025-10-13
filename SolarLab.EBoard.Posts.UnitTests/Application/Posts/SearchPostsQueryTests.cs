using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Search;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Commons;
using static SolarLab.EBoard.Posts.UnitTests.Application.Posts.TestPostConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class SearchPostsQueryTests
{
    private readonly Mock<IPostsQueries> _postsQueriesMock;
    private readonly SearchPostsHandler _handler; 
    
    public SearchPostsQueryTests()
    {
        _postsQueriesMock = new Mock<IPostsQueries>();

        _handler = new SearchPostsHandler(_postsQueriesMock.Object);
    }
    
    [Fact]
    public async Task SearchPosts_WithoutFilters_ReturnsPaginatedPostsResult()
    {
        // Arrange
        var request = new SearchPostsQuery(null, null, null, null, null);

        var expectedPostReadModels = new List<PostReadModel>
        {
            new PostReadModel(
                Guid.Parse("e375614b-31ee-4c7f-9b4d-819b554f5e31"),
                TestTitle,
                TestDescription,
                TestCategoryId,
                TestPrice,
                TestUserId,
                TestDateTime),
            new PostReadModel(
                Guid.Parse("848a6728-300f-4086-adf7-9eeeb8d908da"),
                "Title 2",
                null,
                Guid.Parse("2cd3865b-d6c8-424c-80bd-092ea4b20656"),
                199.99m,
                Guid.Parse("9bf6b9c8-4680-414e-84b6-69a6964637b4"),
                TestDateTime.AddDays(2))
        };

        _postsQueriesMock.Setup(q => q.SearchAsync(
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<PostReadModel>(expectedPostReadModels, 1, 10, 2));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(expectedPostReadModels, result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.TotalCount);
    }

    [Fact]
    public async Task SearchPosts_FilteringByTitle_ReturnsPaginatedPostsResult()
    {
        // Arrange
        var titleFilter = "Title 2";
        var request = new SearchPostsQuery(titleFilter, null, null, null, null);

        var expectedPostReadModels = new List<PostReadModel>
        {
            new PostReadModel(
                Guid.Parse("848a6728-300f-4086-adf7-9eeeb8d908da"),
                "Title 2",
                null,
                Guid.Parse("2cd3865b-d6c8-424c-80bd-092ea4b20656"),
                199.99m,
                Guid.Parse("9bf6b9c8-4680-414e-84b6-69a6964637b4"),
                TestDateTime)
        };

        _postsQueriesMock.Setup(q => q.SearchAsync(
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<PostReadModel>(expectedPostReadModels, 1, 10, 1));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(expectedPostReadModels, result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task SearchPosts_FilteringByCategoryId_ReturnsPaginatedPostsResult()
    {
        // Arrange
        var categoryIdFilter = Guid.Parse("2cd3865b-d6c8-424c-80bd-092ea4b20656");
        var request = new SearchPostsQuery(null, categoryIdFilter, null, null, null);

        var expectedPostReadModels = new List<PostReadModel>
        {
            new PostReadModel(
                Guid.Parse("848a6728-300f-4086-adf7-9eeeb8d908da"),
                "Title 2",
                null,
                Guid.Parse("2cd3865b-d6c8-424c-80bd-092ea4b20656"),
                199.99m,
                Guid.Parse("9bf6b9c8-4680-414e-84b6-69a6964637b4"),
                TestDateTime),
            new PostReadModel(
                Guid.Parse("1cb8351e-0bf8-4ec3-af38-6510e8da7c89"),
                "Title 3",
                "Description 3",
                Guid.Parse("2cd3865b-d6c8-424c-80bd-092ea4b20656"),
                39.99m,
                Guid.Parse("92d0e7bf-2dfe-4e66-8b39-00d32bc9ad75"),
                TestDateTime.AddDays(2))
        };

        _postsQueriesMock.Setup(q => q.SearchAsync(
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<PostReadModel>(expectedPostReadModels, 1, 10, 2));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(expectedPostReadModels, result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.TotalCount);
    }
    
    [Fact]
    public async Task SearchPosts_FilteringByUserId_ReturnsPaginatedPostsResult()
    {
        // Arrange
        var userIdFilter = Guid.Parse("f0912bb8-0c42-403c-9be1-e3c4011f86fb");
        var request = new SearchPostsQuery(null, null, userIdFilter, null, null);

        var expectedPostReadModels = new List<PostReadModel>
        {
            new PostReadModel(
                Guid.Parse("848a6728-300f-4086-adf7-9eeeb8d908da"),
                "Title 2",
                null,
                Guid.Parse("2cd3865b-d6c8-424c-80bd-092ea4b20656"),
                199.99m,
                Guid.Parse("f0912bb8-0c42-403c-9be1-e3c4011f86fb"),
                TestDateTime),
            new PostReadModel(
                Guid.Parse("1cb8351e-0bf8-4ec3-af38-6510e8da7c89"),
                "Title 3",
                "Description 3",
                Guid.Parse("6aae16dc-e23a-420d-9074-3d5bbe9606c0"),
                39.99m,
                Guid.Parse("f0912bb8-0c42-403c-9be1-e3c4011f86fb"),
                TestDateTime.AddDays(2))
        };

        _postsQueriesMock.Setup(q => q.SearchAsync(
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<PostReadModel>(expectedPostReadModels, 1, 10, 2));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(expectedPostReadModels, result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.TotalCount);
    }

    [Fact]
    public async Task SearchPosts_FilteringByMinPrice_ReturnsPaginatedPostsResult()
    {
        // Arrange
        var minPriceFilter = 99.99m;
        var request = new SearchPostsQuery(null, null, null, minPriceFilter, null);

        var expectedPostReadModels = new List<PostReadModel>
        {
            new PostReadModel(
                Guid.Parse("848a6728-300f-4086-adf7-9eeeb8d908da"),
                "Title 2",
                null,
                Guid.Parse("2cd3865b-d6c8-424c-80bd-092ea4b20656"),
                199.99m,
                Guid.Parse("f0912bb8-0c42-403c-9be1-e3c4011f86fb"),
                TestDateTime)
        };

        _postsQueriesMock.Setup(q => q.SearchAsync(
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<PostReadModel>(expectedPostReadModels, 1, 10, 1));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(expectedPostReadModels, result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalCount);
    }
    
    [Fact]
    public async Task SearchPosts_FilteringByMaxPrice_ReturnsPaginatedPostsResult()
    {
        // Arrange
        var maxPriceFilter = 99.99m;
        var request = new SearchPostsQuery(null, null, null, null, maxPriceFilter);

        var expectedPostReadModels = new List<PostReadModel>
        {
            new PostReadModel(
                Guid.Parse("1cb8351e-0bf8-4ec3-af38-6510e8da7c89"),
                "Title 3",
                "Description 3",
                Guid.Parse("6aae16dc-e23a-420d-9074-3d5bbe9606c0"),
                39.99m,
                Guid.Parse("f0912bb8-0c42-403c-9be1-e3c4011f86fb"),
                TestDateTime.AddDays(2))
        };

        _postsQueriesMock.Setup(q => q.SearchAsync(
                It.IsAny<string?>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<Guid?>(),
                It.IsAny<decimal?>(),
                It.IsAny<decimal?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<PostReadModel>(expectedPostReadModels, 1, 10, 1));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(expectedPostReadModels, result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalCount);
    }
}