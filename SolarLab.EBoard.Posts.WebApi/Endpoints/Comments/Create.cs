using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Create;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class Create : IEndpoint
{
    internal sealed record CreateCommentRequest(Guid PostId, string Text);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/comments",
            [SwaggerOperation("Create new comment for requested post")]
            [SwaggerResponse(201, "Comment was created successfully", typeof(string))]
            [SwaggerResponse(500, "Internal server error")]
            async (
                CreateCommentRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCommentCommand(request.PostId, request.Text);
                var result = await mediator.Send(command, cancellationToken);
                return Results.CreatedAtRoute(GetById.EndpointName, new { id = result }, result);
            })
            .RequireAuthorization();
    }
}