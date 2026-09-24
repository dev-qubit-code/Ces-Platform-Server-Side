using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Requests
{
    public class UpdateTestRequest
    {
        public Guid TeacherId { set; get; }
        public Guid CourseId { set; get; }
        public TestKind Kind { set; get; }
        public TestStatus Status { set; get; }
        public DateOnly TestDate{ set; get; }
    }
}
