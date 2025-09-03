using MediatR;
using SolarLab.EBoard.Posts.Application.Comments.Create;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class Create : IEndpoint
{
    internal sealed record Request(Guid PostId, string Text);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/comments",
            async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new CreateCommentCommand(request.PostId, request.Text);
                var result = await mediator.Send(command, cancellationToken);
                return Results.CreatedAtRoute(GetById.EndpointName, new { id = result }, result);
            })
            .RequireAuthorization();
    }
}