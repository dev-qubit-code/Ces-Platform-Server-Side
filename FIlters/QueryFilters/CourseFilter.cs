namespace Ces_Platform_Server_Side.FIlters.QueryFilters
{
    public class CourseFilter
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
