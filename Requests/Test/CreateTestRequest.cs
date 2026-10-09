using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Requests
{
    public class CreateTestRequest
    {
        public Guid CourseId { set; get; }
        public Guid TeacherId { set; get; }
        public DateOnly TestDate { set; get; }
        public TestKind kind { set; get; } = TestKind.None;
    }
}
