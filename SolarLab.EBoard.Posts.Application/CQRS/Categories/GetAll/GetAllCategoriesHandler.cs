using MediatR;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.GetAll;

public sealed class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public GetAllCategoriesHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoriesRepository.GetAllAsync(cancellationToken);
        return result.Select(c => new CategoryDto(c.Id, c.Name, c.ParentId));
    }
}