using MediatR;
using SolarLab.EBoard.Posts.Application.Posts.GetAll;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAllPostsQuery(), cancellationToken);
                return Results.Ok(result);
            })
            .AllowAnonymous();
    }
}