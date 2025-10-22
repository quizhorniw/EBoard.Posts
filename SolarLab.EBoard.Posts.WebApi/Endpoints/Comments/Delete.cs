using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Delete;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/comments/{id:guid}",
            [SwaggerOperation("Delete comment by ID")]
            [SwaggerResponse(204, "Deletion was successful")]
            [SwaggerResponse(400, "Unauthorized access")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Comment ID")]
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) => 
            {
                await mediator.Send(new DeleteCommentCommand(id), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}