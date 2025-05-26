using System;

namespace BloggingPlatfromAPI.Helpers;

public class PageResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public IEnumerable<T> Items { get; set; } = new List<T>();
}
