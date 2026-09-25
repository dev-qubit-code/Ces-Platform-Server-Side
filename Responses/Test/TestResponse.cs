using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Responses
{
    public class TestResponse
    {
        public Guid Id { set; get; }
        public string CourseName { set; get; } = string.Empty;
        public string TeacherName { set; get; } = string.Empty;
        public string TestKind { set; get; } = string.Empty;
        public string TestDate { set; get; } = string.Empty;

        public static TestResponse FromModel(Test requset)
        {
            return new TestResponse
            {
                Id = requset.Id,
                CourseName = requset.Course.Name,
                TeacherName = requset.Teacher.Name,
                TestKind = requset.Kind.ToString(),
                TestDate = requset.Date.ToString("o")
            };
        }

        public static IEnumerable<TestResponse> FromModels(IEnumerable<Test> tests)
            => tests.Select(FromModel);
    }
}
