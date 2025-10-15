using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.Create;
using SolarLab.EBoard.Posts.Domain.Entities;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Categories;

public class CreateCategoryCommandTests
{
    private readonly Mock<ICategoriesRepository> _categoriesRepositoryMock;
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryCommandTests()
    {
        _categoriesRepositoryMock = new Mock<ICategoriesRepository>();

        _handler = new CreateCategoryHandler(_categoriesRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreatedCategoryId()
    {
        // Arrange
        var request = new CreateCategoryCommand(TestName, TestParentId);

        var parent = new Category(TestName, TestId);
        _categoriesRepositoryMock
            .Setup(r => r.GetByIdAsync(TestParentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(parent);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }
    
    [Fact]
    public async Task CreateCategory_SetsCreatedCategoryParentId()
    {
        // Arrange
        var parent = new Category(TestName, null);
        var request = new CreateCategoryCommand(TestName, parent.Id);

        _categoriesRepositoryMock
            .Setup(r => r.GetByIdAsync(parent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(parent);

        Category? capturedCategory = null;
        _categoriesRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((c, _) => capturedCategory = c)
            .Returns(Task.CompletedTask);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(capturedCategory);
        Assert.Equal(parent.Id, capturedCategory.ParentId);
    }
    
    [Fact]
    public async Task CreateCategory_SavesCategoryToDatabase()
    {
        // Arrange
        var parent = new Category(TestName, null);
        var request = new CreateCategoryCommand(TestName, parent.Id);

        _categoriesRepositoryMock
            .Setup(r => r.GetByIdAsync(parent.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(parent);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _categoriesRepositoryMock.Verify(r => 
                r.AddAsync(It.Is<Category>(c => 
                        Guid.Empty != c.Id &&
                        TestName == c.Name &&
                        parent.Id == c.ParentId), 
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
}