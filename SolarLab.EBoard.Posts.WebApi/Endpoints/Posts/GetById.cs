using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.GetById;
using SolarLab.EBoard.Posts.Application.ReadModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

internal sealed class GetById : IEndpoint
{
    internal const string EndpointName = "GetPostById";
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts/{id:guid}",
            [SwaggerOperation("Get post by ID")]
            [SwaggerResponse(200, "Success", typeof(PostReadModel))]
            [SwaggerResponse(404, "Requested post was not found")]
            [SwaggerResponse(500, "Internal server error")]
            async ([SwaggerParameter("Post ID")] Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetPostByIdQuery(id), cancellationToken);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName(EndpointName)
            .AllowAnonymous();
    }
}