using MediatR;
using Microsoft.AspNetCore.Authorization;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.Delete;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/categories/{id:guid}",
            [SwaggerOperation("Delete category by ID")]
            [SwaggerResponse(204, "Category was deleted successfully")]
            [SwaggerResponse(400, "Unauthorized access")]
            [SwaggerResponse(500, "Internal server error")]
            async (
                [SwaggerParameter("Category ID")]
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}