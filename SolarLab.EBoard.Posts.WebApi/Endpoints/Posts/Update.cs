using MediatR;
using SolarLab.EBoard.Posts.Application.Posts.Update;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

internal sealed class Update : IEndpoint
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
        app.MapPut("/posts/{id:guid}",
            async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) => 
            {
                var command = new UpdatePostCommand(
                    id,
                    request.Title,
                    request.Description,
                    request.CategoryId,
                    request.Price,
                    request.ImagePath
                    );
                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}