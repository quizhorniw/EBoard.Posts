using Microsoft.EntityFrameworkCore;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Infrastructure.Persistence;

public class CategoriesQueries : ICategoriesQueries
{
    private readonly AppDbContext _context;

    public CategoriesQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CategoryReadModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Select(c => new CategoryReadModel(c.Id, c.Name, c.ParentId))
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return result is not null ? new CategoryReadModel(result.Id, result.Name, result.ParentId) : null;
    }
}