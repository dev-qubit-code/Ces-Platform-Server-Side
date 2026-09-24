using Ces_Platform_Server_Side.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

public class ReportRepository(AppDbContext context) : IReportRepository
{
    public async Task<(int,List<Report>)> GetReportsPageAsync(ReportFilter? filter, CancellationToken ct = default)
    {
        IQueryable<Report> reports = context.Reports;

        List<Report> page;
        int totalCount;

        if(filter is null)
        {
            page = await reports.Take(10).ToListAsync(ct);

            totalCount = await reports.CountAsync();
            return (totalCount,page);
        }

        filter.PageSize = Math.Max(1, filter.PageSize);
        filter.Page = Math.Clamp(filter.Page, 1, 100);

        if(!string.IsNullOrWhiteSpace(filter.Search))
            reports = reports.Where(u =>
                     u.TItle.Contains(filter.Search) || 
                     u.Description.Contains(filter.Search)  
                );

        totalCount = await reports.CountAsync(ct);

        page = await reports.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
        
        return (totalCount,page);
    }

    public async Task<Report?> GetReportByIdAsync(Guid reportId, CancellationToken ct = default)
    {
        return await context.Reports.FirstOrDefaultAsync(u => u.Id == reportId, ct);
    }

    public async Task<bool> AddReportAsync(Report report, CancellationToken ct = default)
    {
        context.Reports.Add(report);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<int> GetReportsCountAsync(CancellationToken ct = default) => await context.Reports.CountAsync(ct);
}