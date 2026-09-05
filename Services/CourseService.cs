using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Repositories.Course;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Responses;
using SPMS_PROJECT.Exceptions;

namespace Ces_Platform_Server_Side.Services
{
    public class CourseService(CourseRepository repo) : ICourseService
    {
        public async Task<CourseResponse> CreateCourse(CreateCourseRequest request, CancellationToken ct = default)
        {
           Course course = Course.Create(request, "test Name"); //i think we should change test name from static to dinamic
            if (!await repo.AddCourseAsync(course, ct))
                throw new InvalidOperationException("Error occured while adding the course");

            return CourseResponse.FromModle(course);
            
        }

        public async Task DeleteCourse(Guid CourseId, CancellationToken ct = default)
        {
            bool sucess  = await repo.DeleteCourseAsync(CourseId, ct);

            if (!sucess)
                throw new InvalidOperationException("Error occurd while deleting the course");
        }

        public async Task<CourseResponse> GetCourseById(Guid CourseId, CancellationToken ct)
        {
            if (CourseId == default(Guid))
                throw new BusinessRuleException("Id is null", StatusCodes.Status404NotFound);
            Course course = await repo.GetCourseByIdAsync(CourseId, ct);

            return course is null ? throw new BusinessRuleException("Course Not found",StatusCodes.Status404NotFound) : CourseResponse.FromModle(course);
        }

        public async Task<PagedResult<CoursePageResponse>> GetPagedCourses(CourseFilter? filter, CancellationToken ct = default)
        {
            (int totalCount, var courses) = await repo.GetCoursePageAsync(filter, ct);

            filter ??= new();
           

            if (courses is null || !courses.Any())
               PagedResult<CoursePageResponse>.Create([], totalCount,filter.Page,filter.PageSize);

            var pagedResult = PagedResult<CoursePageResponse>.Create(
                CoursePageResponse.FromModles(courses),
                totalCount,
                filter.Page,
                filter.PageSize);

            return pagedResult;
        }

        public async Task UpdateCourse(Guid CourseId, UpdateCourseRequest request, CancellationToken ct = default)
        {
            Course course = await repo.GetCourseByIdAsync(CourseId);

            if (course is null)
                throw new BusinessRuleException("course not found", StatusCodes.Status404NotFound);

            if (course.IsEqual(request))
                throw new BusinessRuleException("course already updated", StatusCodes.Status409Conflict);

            course.Assign(request, "testName");

            if (!await repo.UpdateCourseAsync(ct))
                throw new InvalidOperationException("Error occured while updating the course");
        }
    }
}
