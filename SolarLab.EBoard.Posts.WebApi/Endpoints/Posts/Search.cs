using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Search;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Commons;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class Search : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts/search",
            [SwaggerOperation("Get paginated posts result")]
            [SwaggerResponse(200, "Success", typeof(PagedResult<PostReadModel>))]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Post title alike filter")]
                string? title,
                [SwaggerParameter("Category ID filter")]
                Guid? categoryId,
                [SwaggerParameter("Post author ID filter")]
                Guid? userId,
                [SwaggerParameter("Minimal price filter")]
                decimal? minPrice,
                [SwaggerParameter("Maximum price filter")]
                decimal? maxPrice,
                [SwaggerParameter("Page number")]
                int page,
                [SwaggerParameter("Posts on one page")]
                int pageSize,
                IMediator mediator,
                CancellationToken cancellationToken) => 
            {
                var query = new SearchPostsQuery(
                    title,
                    categoryId,
                    userId,
                    minPrice,
                    maxPrice,
                    page,
                    pageSize);
                
                var result = await mediator.Send(query, cancellationToken);
                return Results.Ok(result);
            })
            .AllowAnonymous();
    }
}