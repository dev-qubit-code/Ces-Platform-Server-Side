using System.Security.Claims;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;

namespace Ces_Platform_Server_Side.Interfaces;

public interface ILogService
{
    public Task<PagedResult<LogPageResponse>> GetPagedLogs(LogFilter? filter, ClaimsPrincipal user, CancellationToken ct = default);
}