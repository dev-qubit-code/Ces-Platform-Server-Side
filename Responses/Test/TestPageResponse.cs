using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses
{
    public class TestPageResponse
    {

        public Guid Id { get; set; }
        public TestStatus Status { get; set; }
        public Guid TeacherId { set; get; }
        public Guid CourseId { set; get; }
        public string CourseName { set; get; } = string.Empty;
        public string TeacherName { set; get; } = string.Empty;
        public DateOnly Date { get; set; }
        public TestKind Kind { get; set; }
        

        public static TestPageResponse FromModel(Test test)
        {
            return new TestPageResponse
            {
                Id = test.Id,
                Status = test.Status,
                CourseName = test.Course.Name,
                TeacherName = test.Teacher.Name,
                Kind = test.Kind,
                Date = test.Date,
                TeacherId  = test.TeacherId,
                CourseId = test.CourseId,
            };
        }

        public static IEnumerable<TestPageResponse> FromModels(IEnumerable<Test> tests)
            => tests.Select(FromModel);

    }


}
