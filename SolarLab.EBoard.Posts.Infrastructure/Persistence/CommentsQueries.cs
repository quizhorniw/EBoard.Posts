using Microsoft.EntityFrameworkCore;
using SolarLab.EBoard.Posts.Application.Abstractions.Persistence;
using SolarLab.EBoard.Posts.Application.ReadModels;

namespace SolarLab.EBoard.Posts.Infrastructure.Persistence;

public class CommentsQueries : ICommentsQueries
{
    private readonly AppDbContext _context;

    public CommentsQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CommentReadModel>> GetByPostIdAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Select(c => new CommentReadModel(c.Id, c.PostId, c.UserId, c.Text))
            .ToListAsync(cancellationToken);
    }

    public async Task<CommentReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return result is not null ? new CommentReadModel(result.Id, result.PostId, result.UserId, result.Text) : null;
    }
}