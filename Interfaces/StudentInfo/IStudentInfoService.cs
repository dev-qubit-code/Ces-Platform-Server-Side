using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;

namespace Ces_Platform_Server_Side.Interfaces;

public interface IStudentInfoService
{
    public  Task<StudentInfoResponse> CreateStudentInfo(CreateStudentInfoRequest request, CancellationToken ct = default);
    public  Task UpdateStudentInfo(Guid studentInfoId,UpdateStudentInfoRequest request, CancellationToken ct = default);
    public  Task<PagedResult<StudentInfoPageResponse>> GetPagedStudentInfos(StudentInfoFilter? filter, CancellationToken ct = default);
    public  Task<StudentInfoResponse> GetStudentInfoById(Guid studentInfoId,CancellationToken ct);
    public  Task DeleteStudentInfo(Guid studentInfoId, CancellationToken ct = default);
}