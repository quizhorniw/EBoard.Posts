using MediatR;
using Microsoft.AspNetCore.Authorization;
using SolarLab.EBoard.Posts.Application.Categories.Update;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

internal sealed class Update : IEndpoint
{
    internal sealed record Request(string Name, Guid? ParentId);
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/categories/{id:guid}", async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new UpdateCategoryCommand(id, request.Name, request.ParentId), cancellationToken);
                return Results.NoContent();
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}