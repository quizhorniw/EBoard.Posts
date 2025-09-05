using Microsoft.EntityFrameworkCore;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;

namespace SolarLab.EBoard.Posts.WebApi.Extensions;

public static class DatabaseExtensions
{
    public static async void MigrateDb(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}