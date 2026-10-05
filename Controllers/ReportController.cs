using Asp.Versioning;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Ces_Platform_Server_Side.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/reports")]
[ApiVersion("1.0")]
[Tags("Reports")]
public class ReportController(IReportService reportService) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<ReportResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("CreateReport")]
    [EndpointSummary("Create report")]
    [EndpointDescription("Create report")]
    public async Task<ActionResult<ReportResponse>> CreateReport(CreateReportRequest request, CancellationToken ct = default) 
    {
        var reportResponse = await reportService.CreateReport(request, ct);

        return CreatedAtAction(nameof(GetReportById), new { reportId = reportResponse.Id }, reportResponse);
    } 


    [HttpGet("{reportId}")]
    [Authorize("Manager/Admin")]
    [Consumes("application/json")]
    [ProducesResponseType<ReportResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetReportById")]
    [EndpointSummary("Get report by id")]
    [EndpointDescription("Get report by id")]
    public async Task<ActionResult<ReportResponse>> GetReportById(Guid reportId, CancellationToken ct = default) 
    {
        var reportResponse = await reportService.GetReportById(reportId, ct);

        return Ok(reportResponse);
    }
    
    [HttpGet]
    [Authorize("Manager/Admin")]
    [Consumes("application/json")]
    [ProducesResponseType<List<ReportResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetReportPage")]
    [EndpointSummary("Get a page of reports")]
    [EndpointDescription("Get the page reports")]
    public async Task<ActionResult<PagedResult<ReportResponse>>> GetReportPage([FromQuery]ReportFilter? filter,CancellationToken ct = default) 
    {
        var reportsPageResponse = await reportService.GetPagedReports(filter,ct);

        return Ok(reportsPageResponse);
    }
}
