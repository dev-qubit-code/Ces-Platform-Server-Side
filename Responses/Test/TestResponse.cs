using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Responses
{
    public class TestResponse
    {
        public Guid Id { set; get; }
        public DateOnly Date { get; set; }
        public TestKind Kind { get; set; }
        public TestStatus Status { get; set; }
        public Guid TeacherId { set; get; }
        public Guid CourseId { set; get; }
        public string TeacherName { set; get; } = string.Empty;
        public string CourseName { set; get; } = string.Empty;

        public static TestResponse FromModel(Test test)
        {
            return new TestResponse
            {
                Id = test.Id,
                Kind = test.Kind,
                Status = test.Status,
                Date = test.Date,
                TeacherId  = test.TeacherId,
                CourseId = test.CourseId,
                TeacherName = test.Teacher.Name,
                CourseName = test.Course.Name
            };
        }

        public static IEnumerable<TestResponse> FromModels(IEnumerable<Test> tests)
            => tests.Select(FromModel);
    }
}
