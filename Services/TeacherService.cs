using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using System.Security.Claims;
using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Services;
public class TeacherService(ITeacherRepository repository,ILoggerWrapper<Teacher> logger,IHttpContextAccessor accessor) : ITeacherService 
{
    public async Task<TeacherResponse> CreateTeacher(CreateTeacherRequest request, CancellationToken ct = default)
    {
        var principal = accessor.HttpContext!.User;

        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);
        
        var teacher =  Teacher.Create(request,username!);

        if(!await repository.AddTeacherAsync(teacher,ct))
        {   
            logger.LogError($"Error occured while adding a new teacher {teacher.Id} by {username} at {DateTime.Now}", userRole);

            throw new InvalidOperationException("Error occured while adding the teacher");
        }

        logger.LogInformation($"Create new teacher {teacher.Id} by {username} at {teacher.CreatedAtUtc}",userRole);

        return TeacherResponse.FromModel(teacher);
    } 
    
    public async Task UpdateTeacher(Guid teacherId,UpdateTeacherRequest request, CancellationToken ct = default)
    {   
        var principal = accessor.HttpContext!.User;
        
        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);    

        var teacher = await repository.GetTeacherByIdAsync(teacherId);

        if(teacher is null)
        {
            logger.LogWarning($"Teacher {teacherId} not found at {DateTime.Now} requested by {username}",userRole);

            throw new BusinessRuleException("Teacher not found",StatusCodes.Status404NotFound);
        }

        if(teacher.IsEqual(request))
            return;
        teacher.Assign(request,"testName");
 
        if(!await repository.UpdateTeacherAsync(ct))
        {
            logger.LogError($"Error occured while updating the teacher {teacher.Id} at {DateTime.Now} by {username}",userRole);

            throw new InvalidOperationException("Error occured while updating the teacher");
        }

        logger.LogInformation($"updated teacher {teacher.Id} by {username} at {teacher.LastModifiedAtUtc}",userRole);

    } 

    public async Task<PagedResult<TeacherPageResponse>> GetPagedTeachers(TeacherFilter? filter, CancellationToken ct = default)
    {
         
        
        (int totalCount,var teachers) = await repository.GetTeachersPageAsync(filter, ct);

        filter ??= new();

        if(teachers is null || !teachers.Any()) 
            return PagedResult<TeacherPageResponse>.Create(
            [],
            totalCount,
            filter.Page,
            filter.PageSize);

        var pagedResult = PagedResult<TeacherPageResponse>.Create(
            TeacherPageResponse.FromModels(teachers),
            totalCount,
            filter.Page,
            filter.PageSize);

        return pagedResult;
    }
    public async Task<TeacherResponse> GetTeacherById(Guid teacherId,CancellationToken ct)
    {
        var principal = accessor.HttpContext!.User;
        
        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);   

        var teacher = await repository.GetTeacherByIdAsync(teacherId,ct);
        
        if(teacher is null)
        {
            logger.LogWarning($"Teacher {teacherId} not found at {DateTime.Now} requested by {username}",userRole);

            throw new BusinessRuleException("Teacher not found",StatusCodes.Status404NotFound);
        }

        return TeacherResponse.FromModel(teacher);
    } 

    public async Task DeleteTeacher(Guid teacherId, CancellationToken ct = default)
    {
        var principal = accessor.HttpContext!.User;
        
        var username = principal!.FindFirstValue(ClaimTypes.GivenName);
        var userRole = Enum.Parse<UserRole>(principal!.FindFirstValue(ClaimTypes.Role)!);    

        var teacher = await repository.GetTeacherByIdAsync(teacherId);

        if(teacher is null)
        {
            logger.LogWarning($"Teacher {teacherId} not found at {DateTime.Now} requested by {username}",userRole);

            throw new BusinessRuleException("Teacher not found",StatusCodes.Status404NotFound);
        }

        if(!await repository.DeleteTeacherAsync(teacher, ct))
        {
            logger.LogError($"Error occured while deleting the teacher {teacher.Id} at {DateTime.Now} by {username}",userRole);  

            throw new InvalidOperationException("Error occurd while deleting the teacher");
        }

        logger.LogInformation($"deleted teacher {teacher.Id} by {username} at {DateTime.Now}",userRole);
    }

}
