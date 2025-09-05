using MediatR;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Application.CQRS.Categories.Create;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public CreateCategoryHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name, null);
        
        var parent = request.ParentId.HasValue 
            ? await _categoriesRepository.GetByIdAsync(request.ParentId.Value, cancellationToken) 
            : null;
        category.SetParent(parent?.Id ?? null);
        
        await _categoriesRepository.AddAsync(category, cancellationToken);

        return category.Id;
    }
}