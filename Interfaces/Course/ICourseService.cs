using Ces_Platform_Server_Side.Requests.Course;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Responses.Course;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

namespace Ces_Platform_Server_Side.Interfaces.Course
{
    public interface ICourseService
    {
        public Task<CourseResponse> CreateCourse(CreateCourseRequest request, CancellationToken ct = default);
        public Task UpdateCourse(Guid CourseId, UpdateCourseRequest request, CancellationToken ct = default);
        public Task<PagedResult<CoursePageResponse>> GetPagedCourses(CourseFilter? filter, CancellationToken ct = default);
        public Task<CourseResponse> GetCourseById(Guid CourseId, CancellationToken ct);
        public Task DeleteCourse(Guid CourseId, CancellationToken ct = default);
       
    }
}
