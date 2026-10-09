using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Interfaces;

public interface ILogRepository
{
    public Task<(int,List<Log>)> GetLogsPageAsync(LogFilter? filter,UserRole role, CancellationToken ct = default);
    public bool AddLog(Log user);
}
