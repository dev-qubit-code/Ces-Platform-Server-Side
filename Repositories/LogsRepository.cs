using Ces_Platform_Server_Side.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Enums;

public class LogRepository(AppDbContext context) : ILogRepository
{
    public bool AddLog(Log log)
    {
        context.Logs.Add(log);
        return context.SaveChanges() > 0;
    }
     public async Task<(int,List<Log>)> GetLogsPageAsync(LogFilter? filter,UserRole role, CancellationToken ct = default)
    {
        IQueryable<Log> logs = context.Logs;

        List<Log> page;
        int totalCount;

        if(filter is null)
        {
            page = await logs.Take(10).ToListAsync(ct);

            totalCount = await logs.CountAsync();
            return (totalCount,page);
        }

        filter.PageSize = Math.Max(1, filter.PageSize);
        filter.Page = Math.Clamp(filter.Page, 1, 100);

        if(role != UserRole.Admin)
            logs = logs.Where(l => l.Role == role);
        

        if(!string.IsNullOrWhiteSpace(filter.Search))
            logs = logs.Where(l => l.Message.Contains(filter.Search));

        totalCount = await logs.CountAsync(ct);

        page = await logs.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
        
        return (totalCount,page);
    }

}
