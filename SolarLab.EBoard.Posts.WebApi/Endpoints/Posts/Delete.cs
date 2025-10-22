using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Delete;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/posts/{id:guid}",
            [SwaggerOperation("Deletes post by ID")]
            [SwaggerResponse(204, "Deletion was successful")]
            [SwaggerResponse(400, "Unauthorized access")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Post ID")] Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeletePostCommand(id), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}