using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Update;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class Update : IEndpoint
{
    public sealed record UpdateCommentRequest(string Text);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/comments/{id:guid}",
            async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var command = new UpdateCommentCommand(id, request.Text);
                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}