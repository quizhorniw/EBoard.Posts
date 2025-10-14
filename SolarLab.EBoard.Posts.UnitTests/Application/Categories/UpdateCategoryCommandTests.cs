using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.Update;
using SolarLab.EBoard.Posts.Domain.Entities;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Categories;

public class UpdateCategoryCommandTests
{
    private readonly Mock<ICategoriesRepository> _categoriesRepositoryMock;
    private readonly UpdateCategoryHandler _handler;

    public UpdateCategoryCommandTests()
    {
        _categoriesRepositoryMock = new Mock<ICategoriesRepository>();
        
        _handler = new UpdateCategoryHandler(_categoriesRepositoryMock.Object);
    }

    [Theory]
    [InlineData("name-1", "b478d324-fa1c-423d-8d52-5715b1ddd30b")]
    [InlineData("name 2", "b9ffe3aa-a72b-41bd-ba14-b68403045b78")]
    [InlineData("NaMe__3", "203d78b5-b7ac-4e59-9606-80e293c020a2")]
    public async Task UpdateCategory_UpdatesCategoryDetails(string name, string? parentId)
    {
        // Arrange
        Guid? parentIdGuid = parentId != null ? Guid.Parse(parentId) : null; 
        var category = new Category(TestName, TestParentId);
        
        var request = new UpdateCategoryCommand(category.Id, name, parentIdGuid);

        _categoriesRepositoryMock
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        
        Category? capturedCategory = null;
        _categoriesRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((c, _) => capturedCategory = c)
            .Returns(Task.CompletedTask);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedCategory);
        Assert.Equal(name, capturedCategory.Name);
        Assert.Equal(parentIdGuid, capturedCategory.ParentId);
    }
    
    [Fact]
    public async Task UpdateCategory_UpdatesCategoryInDatabase()
    {
        // Arrange
        var category = new Category(TestName, TestParentId);
        
        var request = new UpdateCategoryCommand(category.Id, TestName, TestParentId);

        _categoriesRepositoryMock
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _categoriesRepositoryMock.Verify(r => 
                r.UpdateAsync(It.Is<Category>(c => 
                        category.Id == c.Id && 
                        TestName == c.Name && 
                        TestParentId == c.ParentId), 
                    It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateCategory_NotExistingInDatabase_Throws()
    {
        // Arrange
        var request = new UpdateCategoryCommand(TestId, TestName, TestParentId);

        _categoriesRepositoryMock
            .Setup(r => r.GetByIdAsync(TestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }
}