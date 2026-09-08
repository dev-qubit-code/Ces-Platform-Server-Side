namespace Ces_Platform_Server_Side.FIlters.QueryFilters
{
    public class NoteFilter
    {
        public string? Search { set; get; }
        public int Page { set; get; } = 1;
        public int PageSize { set; get; } = 10;
    }
}
