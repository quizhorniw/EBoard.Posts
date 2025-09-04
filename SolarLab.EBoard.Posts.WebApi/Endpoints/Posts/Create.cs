using MediatR;
using SolarLab.EBoard.Posts.Application.Posts.Create;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

internal sealed class Create : IEndpoint
{
    internal sealed record Request(
        string Title,
        string? Description,
        Guid CategoryId,
        decimal Price,
        string? ImagePath
        );
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/posts", async (Request request, IMediator mediator, CancellationToken cancellationToken) => 
            {   
                var command = new CreatePostCommand(
                    request.Title,
                    request.Description,
                    request.CategoryId,
                    request.Price,
                    request.ImagePath
                    );
                var result = await mediator.Send(command, cancellationToken);
                return Results.CreatedAtRoute(GetById.EndpointName, new { id = result }, result);
            })
            .RequireAuthorization();
    }
}