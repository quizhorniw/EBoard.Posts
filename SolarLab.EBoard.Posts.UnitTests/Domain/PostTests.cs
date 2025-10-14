using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Domain.ValueObjects;
using static SolarLab.EBoard.Posts.UnitTests.TestPostConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Domain;

public class PostTests
{
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
            TestDateTime);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(title, result.Title);
        Assert.Equal(description, result.Description);
        Assert.Equal(categoryIdGuid, result.CategoryId);
        Assert.Equal(price, result.Price);
        Assert.Equal(userIdGuid, result.UserId);
        Assert.Equal(TestDateTime, result.CreatedAt);
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
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Post(
            TestUserId,
            title,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-999)]
    [InlineData(-3579)]
    public void CreatePost_WithNegativePrice_Throws(decimal price)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            price,
            TestDateTime));
    }

    [Theory]
    [InlineData("Title 1", "Description 1", "996b8bcc-d09e-4818-b19a-1c458903f041", 11.11)]
    [InlineData("Test title 2", "Some description 2", "5af5f83b-6edb-47a6-9f77-d9247f7ce852", 99.22)]
    [InlineData("Another title 3", null, "c0a85fd9-b86b-415b-957d-4cdb87fe5423", 30.00)]
    [InlineData("4-title", "DeScRiPtIoN | 4", "1a89f33f-a5ad-48b8-81fb-346d84e02ab9", 0.00)]
    [InlineData("t i t l e # 5", "d-e-s-c-r-i-p-t-i-o-n-5", "4bc1387a-14b1-4fb8-bf42-9a3e18522533", 10.01)]
    public void UpdatePostDetails_WithValidData_UpdatesPostDetails(string title, string? description, string categoryId, 
        decimal price)
    {
        // Arrange
        var categoryIdGuid = Guid.Parse(categoryId);
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice, 
            TestDateTime);
        
        // Act
        post.UpdateDetails(title, description, categoryIdGuid, price);
        
        // Assert
        Assert.Equal(TestUserId, post.UserId); // UserId shouldn't have been changed
        Assert.Equal(title, post.Title);
        Assert.Equal(description, post.Description);
        Assert.Equal(categoryIdGuid, post.CategoryId);
        Assert.Equal(price, post.Price);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void UpdatePostDetails_WithNullOrWhitespaceTitle_Throws(string title)
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice, 
            TestDateTime);
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => post.UpdateDetails(title, TestDescription, TestCategoryId, TestPrice));
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-999)]
    [InlineData(-3579)]
    public void UpdatePostDetails_WithNegativePrice_Throws(decimal price)
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice, 
            TestDateTime);
        
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => post.UpdateDetails(TestTitle, TestDescription, TestCategoryId, price));
    }

    [Theory]
    [InlineData("pic1.jpeg", "image/jpeg", 1000)]
    [InlineData("pic2.png", "image/png", 720)]
    [InlineData("pic3-abc.png", "image/png", 590)]
    public void AddImageToPost_AddsImageToPost(string fileName, string mimeType, long size)
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice, 
            TestDateTime);
        
        var image = new Image(fileName, mimeType, size);
        
        // Act
        post.AddImage(image);
        
        // Assert
        Assert.Contains(image, post.Images);
        Assert.Equal(1, post.Images.Count);
    }

    [Fact]
    public void DeleteImageFromPost_DeletesImageFromPost()
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice, 
            TestDateTime);
        
        var image = new Image("pic1.jpeg", "image/jpeg", 1000);
        post.AddImage(image);
        
        // Act
        post.DeleteImage(image);

        // Assert
        Assert.DoesNotContain(image, post.Images);
        Assert.Equal(0, post.Images.Count);
    }
}