using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.AddImages;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class AddImages : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/posts/{id:guid}/images",
            [SwaggerOperation("Add images to post by post ID")]
            [SwaggerResponse(200, "Images were added to post successfully")]
            [SwaggerResponse(404, "Requested post was not found")]
            [SwaggerResponse(400, "Unauthorized access")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Post Id")]
                Guid id,
                HttpRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var files = request.Form.Files.ToList();
                if (!files.Any())
                {
                    return Results.BadRequest("No files uploaded");
                }

                await mediator.Send(new AddImagesToPostCommand(id, files), cancellationToken);
                return Results.Ok();
            })
            .Accepts<IFormFile>("multipart/form-data")
            .RequireAuthorization();
    }
}