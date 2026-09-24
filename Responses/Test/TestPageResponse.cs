using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses
{
    public class TestPageResponse
    {

        public string CourseName { set; get; } = string.Empty;
        public string TeacherName { set; get; } = string.Empty;
        public string TestKind { set; get; } = string.Empty;
        public string TestDate { set; get; } = string.Empty;

        public static TestPageResponse FromModel(Test requset)
        {
            return new TestPageResponse
            {
                CourseName = requset.Course.Name,
                TeacherName = requset.Teacher.Name,
                TestKind = requset.Kind.ToString(),
                TestDate = requset.Date.ToString("o")
            };
        }

        public static IEnumerable<TestPageResponse> FromModels(IEnumerable<Test> tests)
            => tests.Select(FromModel);

    }


}
