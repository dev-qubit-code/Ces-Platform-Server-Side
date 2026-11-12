using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using System.Security.Claims;

namespace Ces_Platform_Server_Side.Services
{
    public class CourseService(ICourseRepository repo, ILoggerWrapper<Teacher> logger, IHttpContextAccessor accessor) : ICourseService
    {
        public async Task<CourseResponse> CreateCourse(CreateCourseRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);


            Course course = Course.Create(request, username!);
            if (!await repo.AddCourseAsync(course, ct))
                throw new InvalidOperationException("Error occured while adding the course");

            logger.LogInformation($"تم انشاء مادة جديده باسم: {course!.Name} بواسطة: {username} في تاريخ: {course!.CreatedAtUtc}", userRole);

            return CourseResponse.FromModel(course);
            
        }

        public async Task DeleteCourse(Guid CourseId, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            var course = await repo.GetCourseByIdAsync(CourseId, ct);

            if (course is null)
                throw new BusinessRuleException("cousrse not found", StatusCodes.Status404NotFound);


            if (!await repo.DeleteCourseAsync(course, ct))
                throw new InvalidOperationException("Error occurd while deleting the course");

                logger.LogInformation($"تم حذف المادة باسم: {course!.Name} بواسطة: {username} في تاريخ: {DateTimeOffset.UtcNow}", userRole);
        }

        public async Task<CourseResponse> GetCourseById(Guid CourseId, CancellationToken ct)
        {
          
            Course? course = await repo.GetCourseByIdAsync(CourseId, ct);

            if(course is null )
            {
                throw new BusinessRuleException("Course Not found", StatusCodes.Status404NotFound); 
            }

          return CourseResponse.FromModel(course);
        }

        public async Task<PagedResult<CoursePageResponse>> GetPagedCourses(CourseFilter? filter, CancellationToken ct = default)
        {
            (int totalCount, var courses) = await repo.GetCoursePageAsync(filter, ct);

            filter ??= new();
           

            if (courses is null || !courses.Any())
             return  PagedResult<CoursePageResponse>.Create([], totalCount,filter.Page,filter.PageSize);

            var pagedResult = PagedResult<CoursePageResponse>.Create(
                CoursePageResponse.FromModles(courses),
                totalCount,
                filter.Page,
                filter.PageSize);

            return pagedResult;
        }

        public async Task UpdateCourse(Guid CourseId, UpdateCourseRequest request, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            Course? course = await repo.GetCourseByIdAsync(CourseId);

            if (course is null)
                throw new BusinessRuleException("course not found", StatusCodes.Status404NotFound);

            if (course.IsEqual(request))
                return;

            course.Assign(request, username!);

            if (!await repo.UpdateCourseAsync(ct))
                throw new InvalidOperationException("Error occured while updating the course");
             
            logger.LogInformation($"تم تعديل المادة باسم: {course!.Name} بواسطة: {username} في تاريخ: {course!.LastModifiedAtUtc}", userRole);
        }
    }
}
