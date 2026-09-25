using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses
{
    public class TestPageResponse
    {

        public string CourseName { set; get; } = string.Empty;
        public string TeacherName { set; get; } = string.Empty;
        public DateOnly Date { get; set; }
        public TestKind Kind { get; set; }

        public static TestPageResponse FromModel(Test requset)
        {
            return new TestPageResponse
            {
                CourseName = requset.Course.Name,
                TeacherName = requset.Teacher.Name,
                Kind = requset.Kind,
                Date = requset.Date
            };
        }

        public static IEnumerable<TestPageResponse> FromModels(IEnumerable<Test> tests)
            => tests.Select(FromModel);

    }


}
