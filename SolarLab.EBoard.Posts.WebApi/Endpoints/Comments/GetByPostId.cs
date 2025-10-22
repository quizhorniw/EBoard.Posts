using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Comments.GetByPostId;
using SolarLab.EBoard.Posts.Application.ReadModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Comments;

internal sealed class GetByPostId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/comments", 
            [SwaggerOperation("Get all post comments by post ID")]
            [SwaggerResponse(200, "Success", typeof(IEnumerable<CommentReadModel>))]
            [SwaggerResponse(404, "Requested comment was not found")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Post ID")]
                Guid postId,
                IMediator mediator,
                CancellationToken cancellationToken) => 
            { 
                var result = await mediator.Send(new GetCommentsByPostIdQuery(postId), cancellationToken); 
                return Results.Ok(result); 
            })
            .AllowAnonymous();
    }
}