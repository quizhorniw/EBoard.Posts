using System.Net;
using System.Text;
using System.Text.Json;
using SolarLab.EBoard.Posts.Application.Abstractions.Http;
using SolarLab.EBoard.Posts.Application.Abstractions.Users;

namespace SolarLab.EBoard.Posts.IntegrationTests;

public class FakeHttpClientProvider : IHttpClientProvider
{
    public Task<HttpResponseMessage> GetAsync(
        string? requestUri, 
        CancellationToken cancellationToken = default)
    {
        var user = new User(
            Guid.NewGuid(),
            "test@mail.com",
            "+71234567890",
            "FirstName",
            "LastName",
            "hash",
            "User",
            "conf-token",
            true);
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonContent = JsonSerializer.Serialize(user, options);

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
        };

        return Task.FromResult(response);
    }
}