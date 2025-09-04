using Microsoft.AspNetCore.Http;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;

namespace SolarLab.EBoard.Posts.Infrastructure.Storage;

public class FileSystemStorageService : IStorageService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    
    public async Task<string> SaveAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var path = Path.Combine(_storagePath, fileName);

        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return fileName;
    }
}