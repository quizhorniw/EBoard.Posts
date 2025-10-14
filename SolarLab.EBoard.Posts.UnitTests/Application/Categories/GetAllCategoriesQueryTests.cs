using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.GetAll;
using SolarLab.EBoard.Posts.Application.ReadModels;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Categories;

public class GetAllCategoriesQueryTests
{
    private readonly Mock<ICategoriesQueries> _categoriesQueriesMock;
    private readonly GetAllCategoriesHandler _handler;

    public GetAllCategoriesQueryTests()
    {
        _categoriesQueriesMock = new Mock<ICategoriesQueries>();

        _handler = new GetAllCategoriesHandler(_categoriesQueriesMock.Object);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsAllCategories()
    {
        // Arrange
        var request = new GetAllCategoriesQuery();

        var categoryReadModels = new List<CategoryReadModel>
        {
            new(
                TestId,
                TestName,
                TestParentId)
        };
        
        _categoriesQueriesMock
            .Setup(q => q.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryReadModels);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(categoryReadModels, result);
    }
}