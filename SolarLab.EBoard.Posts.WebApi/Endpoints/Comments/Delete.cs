using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.Delete;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/comments/{id:guid}", 
            async (Guid id, IMediator mediator, CancellationToken cancellationToken) => 
            {
                await mediator.Send(new DeleteCommentCommand(id), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}