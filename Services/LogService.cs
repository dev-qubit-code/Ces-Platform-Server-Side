using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using System.Security.Claims;
using Ces_Platform_Server_Side.Enums;

public class LogService(ILogRepository repository) : ILogService 
{
    public async Task<PagedResult<LogPageResponse>> GetPagedLogs(LogFilter? filter, ClaimsPrincipal user, CancellationToken ct = default)
    {
         
        var userRole = Enum.Parse<UserRole>(user!.FindFirstValue(ClaimTypes.Role)!);
        
        (int totalCount,var logs) = await repository.GetLogsPageAsync(filter,userRole);

        filter ??= new();

        if(logs is null || !logs.Any()) 
            return PagedResult<LogPageResponse>.Create(
            [],
            totalCount,
            filter.Page,
            filter.PageSize);

        var pagedResult = PagedResult<LogPageResponse>.Create(
            LogPageResponse.FromModels(logs),
            totalCount,
            filter.Page,
            filter.PageSize);

        return pagedResult;
    }
}