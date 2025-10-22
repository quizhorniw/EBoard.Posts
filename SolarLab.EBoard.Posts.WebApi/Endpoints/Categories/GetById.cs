using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.GetById;
using SolarLab.EBoard.Posts.Application.ReadModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

internal sealed class GetById : IEndpoint
{
    internal const string EndpointName = "GetCategoryById";
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories/{id:guid}",
            [SwaggerOperation("Get category by ID")]
            [SwaggerResponse(200, "Success", typeof(CategoryReadModel))]
            [SwaggerResponse(404, "Requested category was not found")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Category ID")] 
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName(EndpointName)
            .AllowAnonymous();
    }
}