using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;

public interface ITeacherService
{
    public Task<TeacherResponse> CreateTeacher(CreateTeacherRequest request, CancellationToken ct = default);
    public Task UpdateTeacher(Guid teacherId,UpdateTeacherRequest request, CancellationToken ct = default);
    public Task<PagedResult<TeacherPageResponse>> GetPagedTeachers(TeacherFilter? filter, CancellationToken ct = default);
    public Task<TeacherResponse> GetTeacherById(Guid teacherId,CancellationToken ct);
    public Task DeleteTeacher(Guid teacherId, CancellationToken ct = default);
}