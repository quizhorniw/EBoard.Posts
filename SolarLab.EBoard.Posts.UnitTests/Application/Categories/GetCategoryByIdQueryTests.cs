using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.GetById;
using SolarLab.EBoard.Posts.Application.ReadModels;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Categories;

public class GetCategoryByIdQueryTests
{
    private readonly Mock<ICategoriesQueries> _categoriesQueriesMock;
    private readonly GetCategoryByIdHandler _handler;

    public GetCategoryByIdQueryTests()
    {
        _categoriesQueriesMock = new Mock<ICategoriesQueries>();
        
        _handler = new GetCategoryByIdHandler(_categoriesQueriesMock.Object);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsFoundCategory()
    {
        // Arrange
        var categoryReadModel = new CategoryReadModel(TestId, TestName, TestParentId);

        var request = new GetCategoryByIdQuery(TestId);

        _categoriesQueriesMock
            .Setup(q => q.GetByIdAsync(TestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryReadModel);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryReadModel, result);
    }
    
    [Fact]
    public async Task GetCategoryById_NotExistingInDatabase_ReturnsNull()
    {
        // Arrange
        var request = new GetCategoryByIdQuery(TestId);

        _categoriesQueriesMock
            .Setup(q => q.GetByIdAsync(TestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CategoryReadModel);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}