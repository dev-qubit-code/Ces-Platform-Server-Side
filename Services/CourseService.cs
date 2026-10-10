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
            {
                logger.LogWarning($"Error occured while adding the course courseId {course.Id} by {username} at {course.CreatedAtUtc}",userRole);
                throw new InvalidOperationException("Error occured while adding the course");
            }
            return CourseResponse.FromModel(course);
            
        }

        public async Task DeleteCourse(Guid CourseId, CancellationToken ct = default)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            bool sucess = await repo.DeleteCourseAsync(CourseId, ct);

            if (!sucess)
            {
                logger.LogError($"Error occured while deleting the course {CourseId} at {DateTime.Now} by {username}", userRole);

                throw new InvalidOperationException("Error occurd while deleting the course");
            }
                logger.LogInformation($"deleted course {CourseId} by {username} at {DateTime.UtcNow}", userRole);
        }

        public async Task<CourseResponse> GetCourseById(Guid CourseId, CancellationToken ct)
        {
            var principal = accessor.HttpContext!.User;

            var username = principal!.FindFirstValue(ClaimTypes.GivenName);
            var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);

            if (CourseId == default(Guid))
                throw new BusinessRuleException("Id is null", StatusCodes.Status404NotFound);
          
            Course? course = await repo.GetCourseByIdAsync(CourseId, ct);

            if(course is null )
            {
                logger.LogWarning($"course {CourseId} not found at {DateTime.Now} requested by {username}", userRole);
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
            {
                logger.LogError($"course is not found {CourseId} at {DateTime.Now} by {username}", userRole);
                throw new BusinessRuleException("course not found", StatusCodes.Status404NotFound);
            }
            if (course.IsEqual(request))
                return;

            course.Assign(request, username!);

            if (!await repo.UpdateCourseAsync(ct))
            {
                logger.LogError($"Error occured while updating the course {course.Id} at {DateTime.Now} by {username}", userRole);

                throw new InvalidOperationException("Error occured while updating the course");
            }else
            {
                logger.LogInformation($"updated course {course.Id}  by {username} at {course.LastModifiedAtUtc}", userRole);
            }
        }
    }
}
