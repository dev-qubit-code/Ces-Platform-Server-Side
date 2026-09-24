using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Interfaces;

public interface IStudentInfoRepository
{
    public Task<(int,List<StudentInfo>)> GetStudentInfosPageAsync(StudentInfoFilter? filter, CancellationToken ct = default);
    public Task<StudentInfo?> GetStudentInfoByIdAsync(Guid studentInfoId, CancellationToken ct = default);
    public  Task<bool> AddStudentInfoAsync(StudentInfo studentInfo, CancellationToken ct = default);
    public Task<bool> UpdateStudentInfoAsync(CancellationToken ct = default);
    public Task<bool> DeleteStudentInfoAsync(StudentInfo studentInfo, CancellationToken ct = default);
    public Task<int> GetStudentInfosCountAsync(CancellationToken ct = default);
    public Task<IEnumerable<Skill>> GetOldSkills(IEnumerable<string> skills, CancellationToken ct = default);
}