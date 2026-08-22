public class UserFilter
{
    public string? Search { get; set; }
    public int Page { get; set => Math.Clamp(value, 1, 100); } = 1;
    public int PageSize { get; set => Math.Max(1, value); } = 10;
}