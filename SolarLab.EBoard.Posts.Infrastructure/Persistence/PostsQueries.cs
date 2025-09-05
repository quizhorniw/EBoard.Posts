using Microsoft.EntityFrameworkCore;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;
using SolarLab.EBoard.Posts.Domain.Commons;

namespace SolarLab.EBoard.Posts.Infrastructure.Persistence;

public class PostsQueries : IPostsQueries
{
    private readonly AppDbContext _context;

    public PostsQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<PostReadModel>> SearchAsync(string? title,
        Guid? categoryId,
        Guid? userId,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(p => EF.Functions.Like(p.Title, $"%{title}%"));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (userId.HasValue)
        {
            query = query.Where(p => p.UserId == userId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }
        
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PostReadModel(
                p.Id, 
                p.Title,
                p.Description,
                p.CategoryId,
                p.Price,
                p.UserId,
                p.CreatedAt
                ))
            .ToListAsync(cancellationToken);
        
        return new PagedResult<PostReadModel>(items, page, pageSize, totalCount);
    }
    
    public async Task<PostReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (result is null) return null;

        return new PostReadModel(
            result.Id,
            result.Title,
            result.Description,
            result.CategoryId,
            result.Price,
            result.UserId,
            result.CreatedAt);
    }
}