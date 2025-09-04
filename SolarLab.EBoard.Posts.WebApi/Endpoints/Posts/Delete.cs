using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Delete;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/posts/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeletePostCommand(id), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}