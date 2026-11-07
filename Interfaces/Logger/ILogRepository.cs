using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Responses;

namespace Ces_Platform_Server_Side.Interfaces;

public interface ILogRepository
{
    public Task<(int,List<LogPageResponse>)> GetLogsPageAsync(LogFilter? filter,UserRole role, CancellationToken ct = default);
    public bool AddLog(Log user);
    public List<Log> GetLastFiveLogs(UserRole role);
}
