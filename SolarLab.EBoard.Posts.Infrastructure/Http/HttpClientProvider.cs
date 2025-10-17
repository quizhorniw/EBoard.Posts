using SolarLab.EBoard.Posts.Application.Abstractions.Http;

namespace SolarLab.EBoard.Posts.Infrastructure.Http;

public class HttpClientProvider : IHttpClientProvider
{
    private readonly HttpClient _httpClient;

    public HttpClientProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetAsync(string? requestUri, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync(requestUri, cancellationToken);
    }
}