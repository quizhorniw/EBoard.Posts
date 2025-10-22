using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Update;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

internal sealed class Update : IEndpoint
{
    internal sealed record UpdatePostRequest(
        string Title,
        string? Description,
        Guid CategoryId,
        decimal Price
    );
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/posts/{id:guid}",
            [SwaggerOperation("Updates post content by post ID")]
            [SwaggerResponse(204, "Update was successful")]
            [SwaggerResponse(404, "Requested post was not found")]
            [SwaggerResponse(400, "Unauthorized access")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Post ID")]
                Guid id, 
                [SwaggerRequestBody("""
                                    Sample request:
                                        
                                        PUT /api/posts/{id}
                                        {
                                          "title": "Blender Philips",
                                          "description": "Blender, was never really used",
                                          "categoryId": "a89f6d00-2470-4332-94d0-153825c714f9",
                                          "price": 150.0
                                        }
                                    """)]
                UpdatePostRequest request, 
                IMediator mediator, 
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePostCommand(
                    id,
                    request.Title,
                    request.Description,
                    request.CategoryId,
                    request.Price);
                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization();
    }
}