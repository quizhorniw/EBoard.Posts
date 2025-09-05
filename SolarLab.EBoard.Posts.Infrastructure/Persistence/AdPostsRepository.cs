using Microsoft.EntityFrameworkCore;
using SolarLab.EBoard.Posts.Domain.Entities;
using SolarLab.EBoard.Posts.Domain.Interfaces;

namespace SolarLab.EBoard.Posts.Infrastructure.Persistence;

public class PostsRepository : IPostsRepository
{
    private readonly AppDbContext _context;

    public PostsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> SearchAsync(
        string? title,
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

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        await _context.Posts.AddAsync(post, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var post = await GetByIdAsync(id, cancellationToken);
        if (post is null) return;
        
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync(cancellationToken);
    }
}