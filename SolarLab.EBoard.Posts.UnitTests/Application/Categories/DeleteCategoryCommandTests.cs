using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.Delete;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Categories;

public class DeleteCategoryCommandTests
{
    private readonly Mock<ICategoriesRepository> _categoriesRepositoryMock;
    private readonly DeleteCategoryHandler _handler;

    public DeleteCategoryCommandTests()
    {
        _categoriesRepositoryMock = new Mock<ICategoriesRepository>();

        _handler = new DeleteCategoryHandler(_categoriesRepositoryMock.Object);
    }

    [Fact]
    public async Task DeleteCategory_DeletesCategoryInDatabase()
    {
        // Arrange
        var request = new DeleteCategoryCommand(TestId);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _categoriesRepositoryMock.Verify(r => r.DeleteAsync(TestId, It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}