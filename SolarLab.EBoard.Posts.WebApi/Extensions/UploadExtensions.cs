using Microsoft.Extensions.FileProviders;

namespace SolarLab.EBoard.Posts.WebApi.Extensions;

public static class UploadExtensions
{
    public static void AddStaticFiles(this WebApplication app)
    {
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }
        
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/uploads"
        });
    }
}