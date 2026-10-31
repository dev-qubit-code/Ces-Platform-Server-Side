using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.FIlters.QueryFilters
{
    public class TestFilter
    {
        public string? Search { set; get; } = string.Empty;
        public int Page { set; get; } = 1;
        public int PageSize { set; get; } = 10;
        public string SortColumn { get; set; } = "createdAt";
        public string SortDirection { get; set; } = "desc";
        public TestStatus Status { set; get; } = TestStatus.None;
        public TestKind TestKind { set; get; } = TestKind.None;
        public int Year { set; get; } = DateTimeOffset.UtcNow.Year;
    }
}
