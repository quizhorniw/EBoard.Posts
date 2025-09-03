using MediatR;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.Categories.GetById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public GetCategoryByIdHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoriesRepository.GetByIdAsync(request.Id, cancellationToken);
        return result is not null ? new CategoryDto(result.Id, result.Name, result.ParentId) : null;
    }
}