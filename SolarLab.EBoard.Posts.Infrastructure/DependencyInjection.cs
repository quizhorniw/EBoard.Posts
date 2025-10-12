using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;
using SolarLab.EBoard.Posts.Application.Abstractions.Time;
using SolarLab.EBoard.Posts.Infrastructure.Authentication;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;
using SolarLab.EBoard.Posts.Infrastructure.Storage;
using SolarLab.EBoard.Posts.Infrastructure.Time;

namespace SolarLab.EBoard.Posts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddServices()
            .AddDatabase(configuration)
            .AddAuthenticationInternal(configuration);
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, FileSystemStorageService>();
        services.AddScoped<IUrlProvider, FileSystemUrlProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts => opts
            .UseNpgsql(configuration.GetConnectionString("PostgresPostsDB"))
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IPostsRepository, PostsRepository>();
        services.AddScoped<IPostsQueries, PostsQueries>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<ICategoriesQueries, CategoriesQueries>();
        services.AddScoped<ICommentsRepository, CommentsRepository>();
        services.AddScoped<ICommentsQueries, CommentsQueries>();
        
        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}