using MediatR;
using Microsoft.AspNetCore.Authorization;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.Update;
using SolarLab.EBoard.Posts.Application.ReadModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

internal sealed class Update : IEndpoint
{
    internal sealed record UpdateCategoryRequest(string Name, Guid? ParentId);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/categories/{id:guid}",
            [SwaggerOperation("Get category by ID")]
            [SwaggerResponse(200, "Success", typeof(CategoryReadModel))]
            [SwaggerResponse(404, "Requested category was not found")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Category ID")]
                Guid id,
                UpdateCategoryRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new UpdateCategoryCommand(id, request.Name, request.ParentId), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}