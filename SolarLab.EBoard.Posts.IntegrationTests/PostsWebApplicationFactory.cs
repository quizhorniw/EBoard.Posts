using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;
using SolarLab.EBoard.Posts.IntegrationTests.Helpers;

namespace SolarLab.EBoard.Posts.IntegrationTests;

public class PostsWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly bool _withAuth;

    public PostsWebApplicationFactory(bool withAuth)
    {
        _withAuth = withAuth;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            WebApplicationFactoryHelper.RemoveDbContext<AppDbContext>(services);
            
            services.AddSingleton<DbConnection>(_ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            if (_withAuth)
            {
                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", opts => { });
            }

            services.AddDbContext<AppDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });

            services.AddHostedService<DatabaseInitializerHostedService>();
        });

        builder.UseEnvironment("IntegrationTests");
    }
}