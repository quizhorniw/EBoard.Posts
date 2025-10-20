using System.Net.Http.Json;
using SolarLab.EBoard.Posts.Application.Abstractions.Http;
using SolarLab.EBoard.Posts.Application.Abstractions.Users;

namespace SolarLab.EBoard.Posts.Infrastructure.Users;

public class UsersService : IUsersService
{
    private readonly IHttpClientProvider _httpClientProvider;

    public UsersService(IHttpClientProvider httpClientProvider)
    {
        _httpClientProvider = httpClientProvider;
    }

    public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var requestUrl = $"{Environment.GetEnvironmentVariable("IDENTITY_SERVICE_URL")}/api/users/{userId}";
        var response = await _httpClientProvider.GetAsync(requestUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<User>(cancellationToken);
        return user;
    }
}

public class Url
{
    public string? Value { get; set; }
}