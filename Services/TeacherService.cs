using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using SPMS_PROJECT.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Services;
public class TeacherService(ITeacherRepository repository) : ITeacherService 
{
    public async Task<TeacherResponse> CreateTeacher(CreateTeacherRequest request, CancellationToken ct = default)
    {
        
        var teacher =  Teacher.Create(request,"testName");

        if(!await repository.AddTeacherAsync(teacher,ct))
            throw new InvalidOperationException("Error occured while adding the teacher");

        return TeacherResponse.FromModel(teacher);
    } 
    
    public async Task UpdateTeacher(Guid teacherId,UpdateTeacherRequest request, CancellationToken ct = default)
    {   
        var teacher = await repository.GetTeacherByIdAsync(teacherId);

        if(teacher is null)
            throw new BusinessRuleException("Teacher not found",StatusCodes.Status404NotFound);

        if(teacher.IsEqual(request))
            throw new BusinessRuleException("teacher already updated",StatusCodes.Status409Conflict);

        teacher.Assign(request,"testName");
 
        if(!await repository.UpdateTeacherAsync(ct))
            throw new InvalidOperationException("Error occured while updating the teacher");
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
        var teacher = await repository.GetTeacherByIdAsync(teacherId,ct) ?? throw new BusinessRuleException("Teacher not found",StatusCodes.Status404NotFound); 

        return TeacherResponse.FromModel(teacher);
    } 

    public async Task DeleteTeacher(Guid teacherId, CancellationToken ct = default)
    {
        var success = await repository.DeleteTeacherAsync(teacherId, ct);

        if(!success) 
            throw new InvalidOperationException("Error occurd while deleting the teacher");
    }

}
