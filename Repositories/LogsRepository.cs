using Ces_Platform_Server_Side.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Responses;

public class LogRepository(AppDbContext context) : ILogRepository
{
    public bool AddLog(Log log)
    {
        context.Logs.Add(log);
        return context.SaveChanges() > 0;
    }
     public async Task<(int,List<LogPageResponse>)> GetLogsPageAsync(LogFilter? filter,UserRole role, CancellationToken ct = default)
    {
        IQueryable<Log> logs = context.Logs;

        List<LogPageResponse> page;
        int totalCount;

        if(filter is null)
        {
            page = await logs
            .Select(l => new LogPageResponse{ Message = l.Message, CreatedAtUtc = l.CreatedAtUtc })
            .Take(10).ToListAsync(ct);

            totalCount = await logs
            .CountAsync();
            return (totalCount,page);
        }

        filter.PageSize = Math.Max(1, filter.PageSize);
        filter.Page = Math.Clamp(filter.Page, 1, 100);

        if(role != UserRole.Admin)
            logs = logs.Where(l => l.Role == role);
        

        if(!string.IsNullOrWhiteSpace(filter.Search))
            logs = logs.Where(l => l.Message.Contains(filter.Search));

        totalCount = await logs.CountAsync(ct);

        page = await logs.OrderByDescending(t => t.CreatedAtUtc)
                        .Skip((filter.Page - 1) * filter.PageSize)
                        .Take(filter.PageSize)
                        .Select(l => new LogPageResponse{ Message = l.Message, CreatedAtUtc = l.CreatedAtUtc })
                        .ToListAsync(ct);
        
        return (totalCount,page);
    }

}
