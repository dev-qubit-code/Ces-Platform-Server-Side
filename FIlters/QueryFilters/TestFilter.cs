using System.Data.SqlTypes;

namespace Ces_Platform_Server_Side.FIlters.QueryFilters
{
    public class TestFilter
    {
        public string Search { set; get; } = string.Empty;
        public int Page { set; get; } 
        public int PageSize { set; get; } 
    }
}
