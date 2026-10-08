using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using System.Security.Claims;
using Ces_Platform_Server_Side.Enums;

public class ReportService(IReportRepository repository,ILoggerWrapper<Teacher> logger,IHttpContextAccessor accessor) : IReportService 
{
    public async Task<ReportResponse> CreateReport(CreateReportRequest request, CancellationToken ct = default)
    {

        // logging userRole is manager and userName is Anonymous as default if there is no authorization 
        var userName = "Anonymous";
        var userRole = UserRole.Manager;

        var report =  Report.Create(request,userName);

        if(!await repository.AddReportAsync(report,ct))
        {
            logger.LogError($"Error occured while adding a new report {report.Id} by {userName} at {DateTime.Now}", userRole);

            throw new InvalidOperationException("Error occured while adding the report");
        }

        logger.LogInformation($"Create new report {report.Id} by {userName} at {report.CreatedAtUtc}",userRole);
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
        // logging userRole is manager and userName is Anonymous as default if there is no authorization 
        var userName = "Anonymous";
        var userRole = UserRole.Manager;

        var report = await repository.GetReportByIdAsync(reportId,ct);
        
        if(report is null)
        {
            logger.LogWarning($"Report {reportId} not found at {DateTime.Now} requested by {userName}",userRole);

            throw new BusinessRuleException("Report not found",StatusCodes.Status404NotFound); 
        }
        

        return ReportResponse.FromModel(report);
    } 

}