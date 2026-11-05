using System.Security.Claims;
using Asp.Versioning;
using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v{version:apiVersion}/dashboard")]
[ApiVersion("1.0")]
[Tags("Dashboard")]
[Authorize("Admin")]
public class DashboardController : ControllerBase
{
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<DashboardResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetDashboardStatus")]
    [EndpointSummary("Get dashboard status")]
    [EndpointDescription("Get dashboard status")]
    public async Task<DashboardResponse> GetDashboardStatus(AppDbContext context, ILogService logService) 
    {    
        return new()
        {
            UsersCount = await context.Users.CountAsync(),
            TeachersCount = await context.Teachers.CountAsync(),
            ApprovedTestsCount = await context.Tests.Where(t => t.Status == TestStatus.Approved).CountAsync(),
            PendingTestsCount = await context.Tests.Where(t => t.Status == TestStatus.Pending).CountAsync(),
            NotesCount = await context.Notes.CountAsync(),
            StudentInfoCount = await context.StudentInfos.CountAsync(),
            DashboardLogs = await logService.GetLastFiveLogs(User)
        };
    }
}