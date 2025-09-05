using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Posts.Search;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Posts;

public class Search : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/posts/search", async (
                string? title,
                Guid? categoryId,
                Guid? userId,
                decimal? minPrice,
                decimal? maxPrice,
                int page,
                int pageSize,
                IMediator mediator,
                CancellationToken cancellationToken
            ) =>
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