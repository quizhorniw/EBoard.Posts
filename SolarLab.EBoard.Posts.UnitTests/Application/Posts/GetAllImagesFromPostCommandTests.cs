using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.GetAllImages;
using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Domain.ValueObjects;
using static SolarLab.EBoard.Posts.UnitTests.TestConstants;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Posts;

public class GetAllImagesFromPostCommandTests
{
    private readonly Mock<IPostsRepository> _postsRepositoryMock;
    private readonly Mock<IUrlProvider> _urlProviderMock;
    private readonly GetAllImagesFromPostHandler _handler;
    
    public GetAllImagesFromPostCommandTests()
    {
        _postsRepositoryMock = new Mock<IPostsRepository>();
        _urlProviderMock = new Mock<IUrlProvider>();

        _handler = new GetAllImagesFromPostHandler(_postsRepositoryMock.Object, _urlProviderMock.Object);
    }

    [Fact]
    public async Task GetAllPostImages_ExistingPostInDatabase_GivesAllPostImages()
    {
        // Arrange
        var post = new Post(
            TestUserId,
            TestTitle,
            TestDescription,
            TestCategoryId,
            TestPrice,
            TestDateTime);
        
        var request = new GetAllImagesFromPostCommand(post.Id);

        post.AddImage(new Image("pic1.jpeg", "image/jpeg", 1000));
        post.AddImage(new Image("pic2.png", "image/png", 680));

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);
        _urlProviderMock.Setup(p => p.GetUrl(It.IsAny<string>())).Returns("path/to/image");
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.Equal(post.Images.Count, result.Images.Count);
        for (var i = 0; i < post.Images.Count; ++i)
        {
            var expectedImage = post.Images[i];
            var actualImage = result.Images[i];
            
            Assert.Equal(expectedImage.MimeType, actualImage.MimeType);
            Assert.Equal(expectedImage.Size, actualImage.Size);
            Assert.StartsWith("path/to/image", actualImage.Url);
        }
    }

    [Fact]
    public async Task GetAllPostImages_NotExistingPostInDatabase_Throws()
    {
        // Arrange
        var postId = Guid.Parse("8ee635a2-fd6f-4999-91cf-f9af5e484d28");
        var request = new GetAllImagesFromPostCommand(postId);

        _postsRepositoryMock
            .Setup(r => r.GetByIdAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Post);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }
}