using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.GetAll;
using SolarLab.EBoard.Posts.Application.ReadModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories",
            [SwaggerOperation("Get all categories")]
            [SwaggerResponse(200, "Success", typeof(IEnumerable<CategoryReadModel>))]
            [SwaggerResponse(500, "Internal server error")]
            async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
                return Results.Ok(result);
            })
            .AllowAnonymous();
    }
}