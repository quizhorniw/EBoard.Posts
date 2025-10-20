using System.Net;
using System.Text;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SolarLab.EBoard.Posts.Application.Abstractions.Authentication;
using SolarLab.EBoard.Posts.Application.Abstractions.Http;
using SolarLab.EBoard.Posts.Application.Abstractions.Messaging;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.Abstractions.Storage;
using SolarLab.EBoard.Posts.Application.Abstractions.Time;
using SolarLab.EBoard.Posts.Application.Abstractions.Users;
using SolarLab.EBoard.Posts.Infrastructure.Authentication;
using SolarLab.EBoard.Posts.Infrastructure.ExceptionHandlers;
using SolarLab.EBoard.Posts.Infrastructure.Http;
using SolarLab.EBoard.Posts.Infrastructure.Messaging;
using SolarLab.EBoard.Posts.Infrastructure.Persistence;
using SolarLab.EBoard.Posts.Infrastructure.Storage;
using SolarLab.EBoard.Posts.Infrastructure.Time;
using SolarLab.EBoard.Posts.Infrastructure.Users;

namespace SolarLab.EBoard.Posts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddServices()
            .AddKafka()
            .AddDatabase()
            .AddAuthenticationInternal();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, FileSystemStorageService>();
        services.AddScoped<IUrlProvider, FileSystemUrlProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddHttpClient<IHttpClientProvider, HttpClientProvider>();
        services.AddScoped<IUsersService, UsersService>();
        
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        return services;
    }

    private static IServiceCollection AddKafka(this IServiceCollection services)
    {
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER"),
            ClientId = Dns.GetHostName(),
        };
        services.AddSingleton<IProducer<string, string>>(_ => new ProducerBuilder<string, string>(kafkaConfig).Build());
        
        services.AddSingleton<IMessageProducer, KafkaNotificationProducer>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opts => opts
            .UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"))
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IPostsRepository, PostsRepository>();
        services.AddScoped<IPostsQueries, PostsQueries>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<ICategoriesQueries, CategoriesQueries>();
        services.AddScoped<ICommentsRepository, CommentsRepository>();
        services.AddScoped<ICommentsQueries, CommentsQueries>();
        
        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        Environment.GetEnvironmentVariable("JWT_SECRET")!)),
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}