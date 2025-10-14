using Moq;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Time;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Create;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.UnitTests.Application.Comments;

public class CreateCommentCommandTests
{
    private readonly Mock<ICommentsRepository> _commentsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly CreateCommentHandler _handler;

    public CreateCommentCommandTests()
    {
        _commentsRepositoryMock = new Mock<ICommentsRepository>();
        _userContextMock = new Mock<IUserContext>();
        _dateTimeProvider = new FakeDateTimeProvider(
            new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        _handler = new CreateCommentHandler(
            _commentsRepositoryMock.Object,
            _userContextMock.Object,
            _dateTimeProvider);
    }

    [Fact]
    public async Task CreateComment_ReturnsCreatedCommentId()
    {
        // Arrange
        var postId = Guid.Parse("3fe8ba7f-1511-464d-947f-2973b7a1c6b1");
        var text = "Test Text";

        var request = new CreateCommentCommand(postId, text);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }
    
    [Fact]
    public async Task CreateComment_SavesCommentToDatabase()
    {
        // Arrange
        var postId = Guid.Parse("3fe8ba7f-1511-464d-947f-2973b7a1c6b1");
        var userId = Guid.Parse("7547ac7c-5f1b-4c01-bf41-6e40a897f24f");
        var text = "Test Text";

        var request = new CreateCommentCommand(postId, text);

        _userContextMock.Setup(c => c.UserId).Returns(userId);
        
        // Act
        await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        _commentsRepositoryMock.Verify(r => 
                r.AddAsync(It.Is<Comment>(c => 
                    Guid.Empty != c.Id &&
                    userId == c.UserId &&
                    _dateTimeProvider.UtcNow == c.CreatedAt &&
                    text == c.Text &&
                    postId == c.PostId), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}