using MediatR;
using SolarLab.EBoard.Posts.Application.CQRS.Categories.GetById;

namespace SolarLab.EBoard.Posts.WebApi.Endpoints.Categories;

internal sealed class GetById : IEndpoint
{
    internal const string EndpointName = "GetCategoryById";
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName(EndpointName)
            .AllowAnonymous();
    }
}