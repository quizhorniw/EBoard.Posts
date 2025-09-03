using MediatR;
using SolarLab.EBoard.Posts.Application.Categories.GetAll;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
                return Results.Ok(result);
            })
            .AllowAnonymous();
    }
}