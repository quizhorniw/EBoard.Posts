using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.EBoard.Posts.Application.CQRS.Posts;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Commons;
using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;
using SolarLab.EBoard.Posts.IntegrationTests.Helpers;

namespace SolarLab.EBoard.Posts.IntegrationTests.IntegrationTests;

public class PostEndpointsTests
{
    [Fact]
    public async Task CreatePost_ReturnsCreatedPostIdAndCreated()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: true);
        var client = factory.CreateClient();
        
        Guid categoryId;
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var category = new Category(TestConstantsHelper.TestCategoryName, null);
            categoryId = category.Id;
            
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }
        
        var requestBody = new
        {
            Title = TestConstantsHelper.TestPostTitle,
            Description = TestConstantsHelper.TestPostDescription,
            CategoryId = categoryId,
            Price = TestConstantsHelper.TestPostPrice
        };

        // Act
        var response = await client.PostAsJsonAsync(HttpUrlHelper.PostsUrl, requestBody);
        var result = await response.Content.ReadFromJsonAsync<Guid>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task CreatePost_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();
        
        var requestBody = new
        {
            Title = TestConstantsHelper.TestPostTitle,
            Description = TestConstantsHelper.TestPostDescription,
            CategoryId = TestConstantsHelper.TestCategoryId,
            Price = TestConstantsHelper.TestPostPrice
        };

        // Act
        var response = await client.PostAsJsonAsync(HttpUrlHelper.PostsUrl, requestBody);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        await Assert.ThrowsAsync<JsonException>(() => response.Content.ReadFromJsonAsync<Guid>());
    }

    [Fact]
    public async Task UpdatePost_ReturnsNoContent()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: true);
        var client = factory.CreateClient();

        Guid categoryId;
        Guid postId;
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<AppDbContext>();

            var category = new Category(TestConstantsHelper.TestCategoryName, null);
            categoryId = category.Id;
            
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
            Title = TestConstantsHelper.TestPostTitle,
            Description = TestConstantsHelper.TestPostDescription,
            CategoryId = categoryId,
            Price = TestConstantsHelper.TestPostPrice
        };
        var requestUrl = HttpUrlHelper.PostsUrl + $"/{postId}";

        // Act
        var response = await client.PutAsJsonAsync(requestUrl, requestBody);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdatePost_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();
        
        var requestBody = new
        {
            Title = TestConstantsHelper.TestPostTitle,
            Description = TestConstantsHelper.TestPostDescription,
            CategoryId = TestConstantsHelper.TestCategoryId,
            Price = TestConstantsHelper.TestPostPrice
        };
        var requestUrl = HttpUrlHelper.PostsUrl + $"/{TestConstantsHelper.TestPostId}";

        // Act
        var response = await client.PutAsJsonAsync(requestUrl, requestBody);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeletePost_ReturnsNoContent()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: true);
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
      
        var requestUrl = HttpUrlHelper.PostsUrl + $"/{postId}";

        // Act
        var response = await client.DeleteAsync(requestUrl);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePost_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();
        
        var requestUrl = HttpUrlHelper.PostsUrl + $"/{TestConstantsHelper.TestPostId}";

        // Act
        var response = await client.DeleteAsync(requestUrl);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SearchPosts_ReturnsFoundPostsAndSuccess()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();
        
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
        }

        var requestUrl = $"{HttpUrlHelper.PostsUrl}/search?page=1&pageSize=10";
        
        // Act
        var response = await client.GetAsync(requestUrl);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<PostReadModel>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetPostById_ReturnsFoundPostAndSuccess()
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
        }

        var requestUrl = HttpUrlHelper.PostsUrl + $"/{postId}";
        
        // Act
        var response = await client.GetAsync(requestUrl);
        var result = await response.Content.ReadFromJsonAsync<PostReadModel>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddImagesToPost_ReturnsSuccess()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: true);
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

        var testFileContent = "Test File Content";
        var testFileStream = new MemoryStream(Encoding.UTF8.GetBytes(testFileContent));

        var formContent = new MultipartFormDataContent();

        var fileContent = new StreamContent(testFileStream);
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "images",
            FileName = "pic1.jpeg"
        };
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        formContent.Add(fileContent);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{HttpUrlHelper.PostsUrl}/{postId}/images");
        requestMessage.Content = formContent;
        
        // Act
        var response = await client.SendAsync(requestMessage);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AddImagesToPost_WithoutFilesAttached_ReturnsBadRequest()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: true);
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

        var formContent = new MultipartFormDataContent();
        formContent.Add(new StringContent(string.Empty), "files");
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{HttpUrlHelper.PostsUrl}/{postId}/images");
        requestMessage.Content = formContent;
        
        // Act
        var response = await client.SendAsync(requestMessage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task AddImagesToPost_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        var formContent = new MultipartFormDataContent();
        formContent.Add(new StringContent(string.Empty), "files");
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, 
            $"{HttpUrlHelper.PostsUrl}/{TestConstantsHelper.TestPostId}/images");
        requestMessage.Content = formContent;
        
        // Act
        var response = await client.SendAsync(requestMessage);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAllPostImages_ReturnsAllPostImagesAndSuccess()
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
        }

        var requestUrl = HttpUrlHelper.PostsUrl + $"/{postId}/images";
        
        // Act
        var response = await client.GetAsync(requestUrl);
        var result = await response.Content.ReadFromJsonAsync<List<ImageDto>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
    }
}