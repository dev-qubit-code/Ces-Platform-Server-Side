using Ces_Platform_Server_Side.Models;
 
namespace Ces_Platform_Server_Side.Responses.Course
{
    public class CourseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        //public List<TestResponse> Tests { set; get; } = [];
        //public List<NoteResponse> Notes { set; get; } = [];

        public static CourseResponse FromModle (Models.Course course)
        {
            return new CourseResponse
            {
                Id = course.Id,
                Name = course.Name,
                
            };
       
        }
        public static IEnumerable<CourseResponse> FromModles(IEnumerable<Models.Course> courses) => courses.Select(FromModle); 

    }
}
