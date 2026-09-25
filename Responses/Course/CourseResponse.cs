
namespace Ces_Platform_Server_Side.Responses
{
    public class CourseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static CourseResponse FromModel(Models.Course course)
        {
            return new CourseResponse
            {
                Id = course.Id,
                Name = course.Name,
                
            };
       
        }
        public static IEnumerable<CourseResponse> FromModels(IEnumerable<Models.Course> courses) => courses.Select(FromModel); 

    }
}
