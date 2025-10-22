using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Create;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

internal sealed class Create : IEndpoint
{
    internal sealed record CreatePostRequest(
        string Title,
        string? Description,
        Guid CategoryId,
        decimal Price
        );
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/posts",
            [SwaggerOperation("Creates new post")]
            [SwaggerResponse(201, "Post was created successfully")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerRequestBody("""
                                    Sample request:
                                        
                                        POST /api/posts
                                        {
                                          "title": "Blender Philips",
                                          "description": "Blender, was never really used",
                                          "categoryId": "a89f6d00-2470-4332-94d0-153825c714f9",
                                          "price": 150.0
                                        }
                                    """)]
                CreatePostRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) => 
            {   
                var command = new CreatePostCommand(
                    request.Title,
                    request.Description,
                    request.CategoryId,
                    request.Price
                    );
                var result = await mediator.Send(command, cancellationToken);
                return Results.CreatedAtRoute(GetById.EndpointName, new { id = result }, result);
            })
            .RequireAuthorization();
    }
}