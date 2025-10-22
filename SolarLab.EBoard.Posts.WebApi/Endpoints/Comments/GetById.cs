using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.GetById;
using SolarLab.EBoard.Posts.Application.ReadModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class GetById : IEndpoint
{
    internal const string EndpointName = "GetCommentById";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/comments/{id:guid}",
            [SwaggerOperation("Get comment by ID")]
            [SwaggerResponse(200, "Success", typeof(CommentReadModel))]
            [SwaggerResponse(404, "Requested post was not found")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Comment ID")]
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCommentByIdQuery(id), cancellationToken);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName(EndpointName)
            .AllowAnonymous();
    }
}