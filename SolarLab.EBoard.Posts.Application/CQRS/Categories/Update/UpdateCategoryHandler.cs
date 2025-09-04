using MediatR;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.Update;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public UpdateCategoryHandler(ICategoriesRepository repository)
    {
        _categoriesRepository = repository;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
        {
            throw new KeyNotFoundException("Category not found");
        }
        
        category.Rename(request.Name);
        category.SetParent(request.ParentId);
        
        await _categoriesRepository.UpdateAsync(category, cancellationToken);
    }
}