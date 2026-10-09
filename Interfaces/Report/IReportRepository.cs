using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Interfaces;

public interface IReportRepository
{
    public Task<(int,List<Report>)> GetReportsPageAsync(ReportFilter? filter, CancellationToken ct = default);
    public Task<Report?> GetReportByIdAsync(Guid reportId, CancellationToken ct = default);
    public  Task<bool> AddReportAsync(Report report, CancellationToken ct = default);
    public Task<int> GetReportsCountAsync(CancellationToken ct = default);
}