using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.GetAllImages;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class GetAllImages : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts/{id:guid}/images",
            [SwaggerOperation("Get all post images by post ID")]
            [SwaggerResponse(200, "Success", typeof(ImageDtoList))]
            [SwaggerResponse(404, "Requested post was not found")]
            [SwaggerResponse(500, "Internal server error")]
            async ([SwaggerParameter("Post ID")] Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAllImagesFromPostCommand(id), cancellationToken);
                return Results.Ok(result);
            })
            .AllowAnonymous();
    }
}