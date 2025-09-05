using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.GetById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryReadModel?>
{
    private readonly ICategoriesQueries _categoriesQueries;

    public GetCategoryByIdHandler(ICategoriesQueries categoriesQueries)
    {
        _categoriesQueries = categoriesQueries;
    }

    public async Task<CategoryReadModel?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _categoriesQueries.GetByIdAsync(request.Id, cancellationToken);
    }
}