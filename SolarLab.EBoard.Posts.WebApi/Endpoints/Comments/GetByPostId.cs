using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class GetByPostId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/comments", async (Guid postId, IMediator mediator, CancellationToken cancellationToken) => 
            { 
                var result = await mediator.Send(new GetCommentsByPostIdQuery(postId), cancellationToken); 
                return Results.Ok(result); 
            })
            .AllowAnonymous();
    }
}