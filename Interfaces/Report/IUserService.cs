using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;

namespace Ces_Platform_Server_Side.Interfaces;
public interface IReportService
{
    public Task<ReportResponse> CreateReport(CreateReportRequest request, CancellationToken ct = default);
     public Task<PagedResult<ReportPageResponse>> GetPagedReports(ReportFilter? filter, CancellationToken ct = default);
    public Task<ReportResponse> GetReportById(Guid reportId,CancellationToken ct);
}