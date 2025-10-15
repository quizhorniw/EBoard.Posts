using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;
using SolarLab.EBoard.Posts.IntegrationTests.Helpers;

namespace SolarLab.EBoard.Posts.IntegrationTests.IntegrationTests;

public class CommentEndpointsTests
{
    [Fact]
    public async Task CreateComment_ReturnsCreatedCommentIdAndSuccess()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory();
        var client = factory.CreateClient();

        Guid postId;
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var category = new Category(TestConstantsHelper.TestCategoryName, null);

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var post = new Post(
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestPostTitle,
                TestConstantsHelper.TestPostDescription,
                category.Id,
                TestConstantsHelper.TestPostPrice,
                TestConstantsHelper.TestDateTime);
            postId = post.Id;

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
        }

        var requestBody = new
        {
            PostId = postId,
            Text = TestConstantsHelper.TestCommentText
        };

        // Act
        var response = await client.PostAsJsonAsync(HttpUrlHelper.CommentsUrl, requestBody);
        var result = await response.Content.ReadFromJsonAsync<Guid>();

        // Arrange
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateComment_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(HttpUrlHelper.CommentsUrl, new { });

        // Arrange
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateComment_ReturnsNoContent()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory();
        var client = factory.CreateClient();

        Guid commentId;
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var category = new Category(TestConstantsHelper.TestCategoryName, null);

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var post = new Post(
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestPostTitle,
                TestConstantsHelper.TestPostDescription,
                category.Id,
                TestConstantsHelper.TestPostPrice,
                TestConstantsHelper.TestDateTime);

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            var comment = Comment.Create(
                post.Id,
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestCommentText,
                TestConstantsHelper.TestDateTime);
            commentId = comment.Id;

            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        var requestBody = new
        {
            Text = TestConstantsHelper.TestCommentText
        };
        var requestUrl = HttpUrlHelper.CommentsUrl + $"/{commentId}";

        // Act
        var response = await client.PutAsJsonAsync(requestUrl, requestBody);

        // Arrange
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateComment_NoAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        var requestUrl = HttpUrlHelper.CommentsUrl + $"/{TestConstantsHelper.TestCommentId}";

        // Act
        var response = await client.PutAsJsonAsync(requestUrl, new { });

        // Arrange
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteComment_ReturnsNoContent()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory();
        var client = factory.CreateClient();

        Guid commentId;
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var category = new Category(TestConstantsHelper.TestCategoryName, null);

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var post = new Post(
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestPostTitle,
                TestConstantsHelper.TestPostDescription,
                category.Id,
                TestConstantsHelper.TestPostPrice,
                TestConstantsHelper.TestDateTime);

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            var comment = Comment.Create(
                post.Id,
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestCommentText,
                TestConstantsHelper.TestDateTime);
            commentId = comment.Id;

            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        var requestUrl = HttpUrlHelper.CommentsUrl + $"/{commentId}";

        // Act
        var response = await client.DeleteAsync(requestUrl);

        // Arrange
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteComment_NoAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        var requestUrl = HttpUrlHelper.CommentsUrl + $"/{TestConstantsHelper.TestCommentId}";

        // Act
        var response = await client.DeleteAsync(requestUrl);

        // Arrange
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCommentById_ReturnsFoundCommentAndSuccess()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        Guid commentId;
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var category = new Category(TestConstantsHelper.TestCategoryName, null);

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var post = new Post(
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestPostTitle,
                TestConstantsHelper.TestPostDescription,
                category.Id,
                TestConstantsHelper.TestPostPrice,
                TestConstantsHelper.TestDateTime);

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            var comment = Comment.Create(
                post.Id,
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestCommentText,
                TestConstantsHelper.TestDateTime);
            commentId = comment.Id;

            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        var requestUrl = HttpUrlHelper.CommentsUrl + $"/{commentId}";

        // Act
        var response = await client.GetAsync(requestUrl);
        var result = await response.Content.ReadFromJsonAsync<CommentReadModel>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCommentById_NotPresentInSystem_ReturnsNotFound()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        var requestUrl = HttpUrlHelper.CommentsUrl + $"/{TestConstantsHelper.TestCommentId}";

        // Act
        var response = await client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCommentsByPostId_ReturnsFoundCommentsAndSuccess()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        Guid postId;
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var category = new Category(TestConstantsHelper.TestCategoryName, null);

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var post = new Post(
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestPostTitle,
                TestConstantsHelper.TestPostDescription,
                category.Id,
                TestConstantsHelper.TestPostPrice,
                TestConstantsHelper.TestDateTime);
            postId = post.Id;

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            var comment = Comment.Create(
                post.Id,
                Guid.Parse(TestAuthHandler.SubClaim),
                TestConstantsHelper.TestCommentText,
                TestConstantsHelper.TestDateTime);

            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        var requestUrl = HttpUrlHelper.CommentsUrl + $"?postId={postId}";

        // Act
        var response = await client.GetAsync(requestUrl);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<CommentReadModel>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
    }
}