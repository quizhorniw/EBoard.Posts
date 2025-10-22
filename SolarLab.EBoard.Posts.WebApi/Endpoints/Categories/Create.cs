using MediatR;
using Microsoft.AspNetCore.Authorization;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.Create;
using SolarLab.EBoard.Posts.Application.ReadModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

internal sealed class Create : IEndpoint
{
    internal sealed record CreateCategoryRequest(string Name, Guid? ParentId);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/categories",
            [SwaggerOperation("Get all categories")]
            [SwaggerResponse(200, "Success", typeof(IEnumerable<CategoryReadModel>))]
            [SwaggerResponse(500, "Internal server error")]
            async (
                CreateCategoryRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new CreateCategoryCommand(request.Name, request.ParentId), cancellationToken);
                return Results.CreatedAtRoute(GetById.EndpointName, new { id = result }, result);
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}