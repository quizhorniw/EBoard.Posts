namespace SolarLab.EBoard.Posts.Application.Abstractions.Http;

public interface IHttpClientProvider
{
    Task<HttpResponseMessage> GetAsync(string? requestUri, CancellationToken cancellationToken = default);
}