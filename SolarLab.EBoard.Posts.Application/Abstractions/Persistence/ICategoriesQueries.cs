using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Application.Abstractions.Persistence;

public interface ICategoriesQueries
{
    Task<IReadOnlyList<CategoryReadModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}