using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Update;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class Update : IEndpoint
{
    public sealed record UpdateCommentRequest(string Text);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/comments/{id:guid}",
            [SwaggerOperation("Update comment text by comment ID")]
            [SwaggerResponse(204, "Comment text was successfully updated")]
            [SwaggerResponse(404, "Requested comment was not found")]
            [SwaggerResponse(400, "Unauthorized access")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Comment ID")]
                Guid id,
                UpdateCommentRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateCommentCommand(id, request.Text);
                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}