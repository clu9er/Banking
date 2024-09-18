namespace Banking.Application.Models;

public class PaginatedResult<T>
{
    public int TotalRecords { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public required List<T> Items { get; set; }
}
