using Microsoft.AspNetCore.Http;

namespace SolarLab.EBoard.Posts.Application.Abstractions.Storage;

public interface IStorageService
{
    Task<string> SaveAsync(IFormFile file, CancellationToken cancellationToken = default);
}