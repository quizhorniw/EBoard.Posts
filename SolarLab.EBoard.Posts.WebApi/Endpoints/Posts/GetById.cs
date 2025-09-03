using MediatR;
using SolarLab.EBoard.Posts.Application.Posts.GetById;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

internal sealed class GetById : IEndpoint
{
    internal const string EndpointName = "GetPostById";
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetPostByIdQuery(id), cancellationToken);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName(EndpointName)
            .AllowAnonymous();
    }
}