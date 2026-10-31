namespace Ces_Platform_Server_Side.FIlters.QueryFilters
{
    public class NoteFilter
    {
        public string? Search { set; get; }
        public int Page { set; get; } = 1;
        public int PageSize { set; get; } = 10;
        public string SortColumn { get; set; } = "createdAtUtc";
        public string SortDirection { get; set; } = "desc";
        public string TeacherName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Year { set; get; } = DateTimeOffset.UtcNow.Year;
    }
}
