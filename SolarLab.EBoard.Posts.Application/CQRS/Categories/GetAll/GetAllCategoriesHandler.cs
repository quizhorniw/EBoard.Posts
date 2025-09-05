using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.GetAll;

public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryReadModel>>
{
    private readonly ICategoriesQueries _categoriesQueries;

    public GetAllCategoriesHandler(ICategoriesQueries categoriesQueries)
    {
        _categoriesQueries = categoriesQueries;
    }

    public async Task<IReadOnlyList<CategoryReadModel>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _categoriesQueries.GetAllAsync(cancellationToken);
    }
}