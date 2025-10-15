namespace SolarLab.EBoard.Posts.Domain.Commons;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }

    public PagedResult(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
    {
        if (page <= 0)
        {
            page = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 1;
        }

        if (totalCount < 0)
        {
            totalCount = 0;
        }
        
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}