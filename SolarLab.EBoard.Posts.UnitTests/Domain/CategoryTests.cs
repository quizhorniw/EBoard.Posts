using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Domain;

public class CategoryTests
{
    private const string TestName = "Name"; 
    private static readonly Guid TestParentId = Guid.Parse("60e5f7b0-286e-41d5-b6bd-f905c35833f2"); 
    
    [Theory]
    [InlineData("Electronics", null)]
    [InlineData("Power supplies", "60e5f7b0-286e-41d5-b6bd-f905c35833f2")]
    [InlineData("Hair care", "a8784267-1e85-47e8-9d72-377f0662817c")]
    public void CreateCategory_WithValidData_CreatesCategory(string name, string? parentId)
    {
        // Arrange
        Guid? parentIdGuid = parentId != null ? Guid.Parse(parentId) : null;

        // Act
        var result = new Category(name, parentIdGuid);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(parentIdGuid, result.ParentId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void CreateCategory_WithNullOrWhitespaceName_Throws(string name)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Category(name, TestParentId));
    }

    [Theory]
    [InlineData("Electronics")]
    [InlineData("Power supplies")]
    [InlineData("Hair care")]
    public void RenameCategory_WithValidNewName_RenamesCategory(string newName)
    {
        // Arrange
        var category = new Category(TestName, TestParentId);

        // Act
        category.Rename(newName);
        
        // Assert
        Assert.Equal(newName, category.Name);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData(" \n\r  \t")]
    public void RenameCategory_WithNullOrWhitespaceNewName_Throws(string newName)
    {
        // Arrange
        var category = new Category(TestName, TestParentId);

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => category.Rename(newName));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("60e5f7b0-286e-41d5-b6bd-f905c35833f2")]
    [InlineData("a8784267-1e85-47e8-9d72-377f0662817c")]
    public void SetCategoryParent_SetsCategoryParent(string? parentId)
    {
        // Arrange
        Guid? parentIdGuid = parentId != null ? Guid.Parse(parentId) : null;
        var category = new Category(TestName, TestParentId);
        
        // Act
        category.SetParent(parentIdGuid);
        
        // Assert
        Assert.Equal(parentIdGuid, category.ParentId);
    }
}