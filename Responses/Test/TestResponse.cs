using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Responses
{
    public class TestResponse
    {
        public Guid Id { set; get; }
        public string CourseName { set; get; } = string.Empty;
        public string TeacherName { set; get; } = string.Empty;
        public DateOnly Date { get; set; }
        public TestKind Kind { get; set; }

        public static TestResponse FromModel(Test requset)
        {
            return new TestResponse
            {
                Id = requset.Id,
                CourseName = requset.Course.Name,
                TeacherName = requset.Teacher.Name,
                Kind = requset.Kind,
                Date = requset.Date
            };
        }

        public static IEnumerable<TestResponse> FromModels(IEnumerable<Test> tests)
            => tests.Select(FromModel);
    }
}
