using Asp.Versioning;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/logs")]
[ApiVersion("1.0")]
[Tags("Logs")]
[Authorize("Manager/Admin")]
public class LogController(ILogService logService) : ControllerBase
{
    
    
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<PagedResult<LogPageResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetLogsPage")]
    [EndpointSummary("Get a page of logs")]
    [EndpointDescription("Get the page logs")]
    public async Task<ActionResult<PagedResult<LogPageResponse>>> GetLogsPage([FromQuery]LogFilter? filter) 
    {
        var logsPageResponse = await logService.GetPagedLogs(filter,User);

        return Ok(logsPageResponse);
    }

    
}