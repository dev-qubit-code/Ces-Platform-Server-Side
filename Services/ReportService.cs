using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using SPMS_PROJECT.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

public class ReportService(IReportRepository repository) : IReportService 
{
    public async Task<ReportResponse> CreateReport(CreateReportRequest request, CancellationToken ct = default)
    {
        
        var report =  Report.Create(request,"testName");

        if(!await repository.AddReportAsync(report,ct))
            throw new InvalidOperationException("Error occured while adding the report");

        return ReportResponse.FromModel(report);
    } 
    
    public async Task<PagedResult<ReportPageResponse>> GetPagedReports(ReportFilter? filter, CancellationToken ct = default)
    {
         
        
        (int totalCount,var reports) = await repository.GetReportsPageAsync(filter, ct);

        filter ??= new();

        if(reports is null || !reports.Any()) 
            return PagedResult<ReportPageResponse>.Create(
            [],
            totalCount,
            filter.Page,
            filter.PageSize);

        var pagedResult = PagedResult<ReportPageResponse>.Create(
            ReportPageResponse.FromModels(reports),
            totalCount,
            filter.Page,
            filter.PageSize);

        return pagedResult;
    }
    public async Task<ReportResponse> GetReportById(Guid reportId,CancellationToken ct)
    {
        var report = await repository.GetReportByIdAsync(reportId,ct) ?? throw new BusinessRuleException("Report not found",StatusCodes.Status404NotFound); 

        return ReportResponse.FromModel(report);
    } 

}