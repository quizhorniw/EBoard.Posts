using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SolarLab.EBoard.Posts.Application.Abstractions.Http;
using SolarLab.EBoard.Posts.Application.Abstractions.Users;

namespace SolarLab.EBoard.Posts.Infrastructure.Users;

public class UsersService : IUsersService
{
    private readonly IHttpClientProvider _httpClientProvider;
    private readonly Url _identityServiceUrl; 

    public UsersService(IOptions<Url> identityServiceUrl, IHttpClientProvider httpClientProvider)
    {
        _httpClientProvider = httpClientProvider;
        _identityServiceUrl = identityServiceUrl.Value;
    }

    public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var requestUrl = $"{_identityServiceUrl.Value}/api/users/{userId}";
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