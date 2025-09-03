using MediatR;
using Microsoft.AspNetCore.Authorization;
using SolarLab.EBoard.Posts.Application.Categories.Delete;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/categories/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
                    return Results.NoContent();
                })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}