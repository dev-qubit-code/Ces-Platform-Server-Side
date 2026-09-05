 

namespace Ces_Platform_Server_Side.Responses.Course
{
    public class CoursePageResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CountOfTests { get; set; } = 0;
        public int CountOfNotes { get; set; } = 0;

        public static CoursePageResponse FromModel(Models.Course course)
        {
            return new CoursePageResponse
            {
                Id = course.Id,
                Name = course.Name,
                
            };

        }
        public static IEnumerable<CoursePageResponse> FromModles(IEnumerable<Models.Course> courses) => courses.Select(FromModle);
    }
}
