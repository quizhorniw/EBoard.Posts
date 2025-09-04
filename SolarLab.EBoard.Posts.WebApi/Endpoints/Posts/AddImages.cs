using MediatR;
using SolarLab.EBoard.Posts.Application.Posts.AddImages;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class AddImages : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/posts/{id:guid}/images",
            async (Guid id, HttpRequest request, IMediator mediator, CancellationToken cancellationToken) =>
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