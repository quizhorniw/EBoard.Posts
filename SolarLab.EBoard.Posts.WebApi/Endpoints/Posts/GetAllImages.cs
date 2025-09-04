using MediatR;
using SolarLab.EBoard.Posts.Application.Posts.GetAllImages;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class GetAllImages : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts/{id:guid}/images", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAllImagesFromPostCommand(id), cancellationToken);
                return Results.Ok(result);
            })
            .AllowAnonymous();
    }
}