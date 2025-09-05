using Microsoft.EntityFrameworkCore;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Domain.Entities;

namespace SolarLab.EBoard.Posts.Infrastructure.Persistence;

public class CommentsRepository : ICommentsRepository
{
    private readonly AppDbContext _context;

    public CommentsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTextAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _context.Update(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await GetByIdAsync(id, cancellationToken);
        if (comment is null) return;
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}