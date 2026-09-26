 

namespace Ces_Platform_Server_Side.Responses
{
    public class CoursePageResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TestsCount { get; set; } = 0;
        public int NotesCount { get; set; } = 0;
        public static CoursePageResponse FromModel(Models.Course course)
        {
            return new CoursePageResponse
            {
                Id = course.Id,
                Name = course.Name,
                TestsCount = course.Tests.Count,
                NotesCount = course.Notes.Count
                
            };

        }
        public static IEnumerable<CoursePageResponse> FromModles(IEnumerable<Models.Course> courses) => courses.Select(FromModel);
    }
}
