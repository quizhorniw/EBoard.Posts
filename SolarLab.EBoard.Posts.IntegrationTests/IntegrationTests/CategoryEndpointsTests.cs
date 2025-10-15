using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;
using SolarLab.EBoard.Posts.IntegrationTests.Helpers;

namespace SolarLab.EBoard.Posts.IntegrationTests.IntegrationTests;

public class CategoryEndpointsTests
{
    [Fact]
    public async Task CreateCategory_ReturnsCreatedCategoryIdAndCreated()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(role: "Admin");
        var client = factory.CreateClient();

        var requestBody = new
        {
            Name = TestConstantsHelper.TestCategoryName,
            ParentId = (Guid?)null
        };

        // Act
        var response = await client.PostAsJsonAsync(HttpUrlHelper.CategoriesUrl, requestBody);
        var result = await response.Content.ReadFromJsonAsync<Guid>();
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(Guid.Empty, result);
    }
    
    [Fact]
    public async Task CreateCategory_NotAdministratorRole_ReturnsForbidden()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(HttpUrlHelper.CategoriesUrl, new { });
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        await Assert.ThrowsAsync<JsonException>(() => response.Content.ReadFromJsonAsync<Guid>());
    }

    [Fact]
    public async Task CreateCategory_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(HttpUrlHelper.CategoriesUrl, new { });
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        await Assert.ThrowsAsync<JsonException>(() => response.Content.ReadFromJsonAsync<Guid>());
    }

    [Fact]
    public async Task UpdateCategory_ReturnsNoContent()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(role: "Admin");
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
            Name = TestConstantsHelper.TestCategoryName,
            ParentId = (Guid?)null
        };
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{categoryId}";

        // Act
        var response = await client.PutAsJsonAsync(requestUrl, requestBody);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateCategory_NotAdministratorRole_ReturnsForbidden()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory();
        var client = factory.CreateClient();
        
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{TestConstantsHelper.TestCategoryId}";

        // Act
        var response = await client.PutAsJsonAsync(requestUrl, new { });
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateCategory_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();
        
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{TestConstantsHelper.TestCategoryId}";

        // Act
        var response = await client.PutAsJsonAsync(requestUrl, new { });
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsNoContent()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(role: "Admin");
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
        
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{categoryId}";

        // Act
        var response = await client.DeleteAsync(requestUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCategory_NotAdministratorRole_ReturnsForbidden()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory();
        var client = factory.CreateClient();
        
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{TestConstantsHelper.TestCategoryId}";

        // Act
        var response = await client.DeleteAsync(requestUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCategory_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();
        
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{TestConstantsHelper.TestCategoryId}";

        // Act
        var response = await client.DeleteAsync(requestUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsFoundCategoryAndSuccess()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
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
        
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{categoryId}";

        // Act
        var response = await client.GetAsync(requestUrl);
        var result = await response.Content.ReadFromJsonAsync<CategoryReadModel>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetCategoryById_NotPresentInDatabase_ReturnsNotFound()
    {
        // Arrange
        var factory = new PostsWebApplicationFactory(withAuth: false);
        var client = factory.CreateClient();
        
        var requestUrl = HttpUrlHelper.CategoriesUrl + $"/{TestConstantsHelper.TestCategoryId}";

        // Act
        var response = await client.GetAsync(requestUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllCategories_ReturnsAllCategoriesAndSuccess()
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
        }

        // Act
        var response = await client.GetAsync(HttpUrlHelper.CategoriesUrl);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryReadModel>>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
    }
}