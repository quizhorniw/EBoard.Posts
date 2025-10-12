using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Domain;

public class PostTests
{
    private readonly DateTime _testDateTime = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    [Theory]
    [InlineData("Title 1", "Description 1", "996b8bcc-d09e-4818-b19a-1c458903f041", 11.11, 
        "5e58d996-0994-4fae-8af0-47b60ae4e567")]
    [InlineData("Test title 2", "Some description 2", "5af5f83b-6edb-47a6-9f77-d9247f7ce852", 99.22, 
        "c68a5999-ced5-4f3c-8687-5067819c9d97")]
    [InlineData("Another title 3", null, "c0a85fd9-b86b-415b-957d-4cdb87fe5423", 30.00, 
        "d192cf84-861e-4c99-aa67-bdc6a7370002")]
    [InlineData("4-title", "DeScRiPtIoN | 4", "1a89f33f-a5ad-48b8-81fb-346d84e02ab9", 0.00, 
        "4f37bc92-0ae6-4f21-b105-c8bd811b9f49")]
    [InlineData("t i t l e # 5", "d-e-s-c-r-i-p-t-i-o-n-5", "4bc1387a-14b1-4fb8-bf42-9a3e18522533", 10.01, 
        "61c597ec-a30f-4122-b4e3-28d4c6687ab3")]
    public void CreatePost_WithValidData_CreatesPost(string title, string? description, string categoryId, 
        decimal price, string userId)
    {
        // Arrange
        var categoryIdGuid = Guid.Parse(categoryId);
        var userIdGuid = Guid.Parse(userId);
        
        // Act
        var result = new Post(
            userIdGuid,
            title,
            description,
            categoryIdGuid,
            price,
            _testDateTime);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(title, result.Title);
        Assert.Equal(description, result.Description);
        Assert.Equal(categoryIdGuid, result.CategoryId);
        Assert.Equal(price, result.Price);
        Assert.Equal(userIdGuid, result.UserId);
        Assert.Equal(_testDateTime, result.CreatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void CreatPost_WithNullOrWhitespaceTitle_Throws(string title)
    {
        // Arrange
        var description = "Description";
        var categoryId = Guid.Parse("5e58d996-0994-4fae-8af0-47b60ae4e567");
        var price = 99.99m;
        var userId = Guid.Parse("1a89f33f-a5ad-48b8-81fb-346d84e02ab9");
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Post(
            userId,
            title,
            description,
            categoryId,
            price,
            _testDateTime));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-999)]
    public void CreatePost_WithNegativePrice_Throws(decimal price)
    {
        // Arrange
        var title = "Title";
        var description = "Description";
        var categoryId = Guid.Parse("5e58d996-0994-4fae-8af0-47b60ae4e567");
        var userId = Guid.Parse("1a89f33f-a5ad-48b8-81fb-346d84e02ab9");

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Post(
            userId,
            title,
            description,
            categoryId,
            price,
            _testDateTime));
    }
}