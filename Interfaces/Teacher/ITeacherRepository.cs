using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Interfaces;

public interface ITeacherRepository
{
    public Task<(int,List<Teacher>)> GetTeachersPageAsync(TeacherFilter? filter, CancellationToken ct = default);
    public Task<Teacher?> GetTeacherByIdAsync(Guid teacherId, CancellationToken ct = default);
    public  Task<bool> AddTeacherAsync(Teacher teacher, CancellationToken ct = default);
    public Task<bool> UpdateTeacherAsync(CancellationToken ct = default);
    public Task<bool> DeleteTeacherAsync(Guid teacherId, CancellationToken ct = default);
    public Task<int> GetTeachersCountAsync(CancellationToken ct = default);
}