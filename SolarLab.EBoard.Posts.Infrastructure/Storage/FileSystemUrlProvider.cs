using Microsoft.Extensions.Configuration;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;

namespace SolarLab.EBoard.Posts.Infrastructure.Storage;

public class FileSystemUrlProvider : IUrlProvider
{
    private readonly string _baseUrl;

    public FileSystemUrlProvider(IConfiguration configuration)
    {
        _baseUrl = configuration["FileStorage:BaseUrl"] 
                   ?? throw new InvalidOperationException("BaseUrl not configured");
    }

    public string GetUrl(string fileName) => $"{_baseUrl}/{fileName}";
}