using Asp.Versioning;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/students-infos")]
[ApiVersion("1.0")]
[Tags("StudentInfos")]
public class StudentInfoController(IStudentInfoService studentInfoService) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<StudentInfoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("CreateStudentInfo")]
    [EndpointSummary("Create studentInfo")]
    [EndpointDescription("Create studentInfo")]
    public async Task<ActionResult<StudentInfoResponse>> CreateStudentInfo(CreateStudentInfoRequest request, CancellationToken ct = default) 
    {
        var studentInfoResponse = await studentInfoService.CreateStudentInfo(request, ct);

        return CreatedAtAction(nameof(GetStudentInfoById), new { studentInfoId = studentInfoResponse.Id }, studentInfoResponse);
    } 

    [HttpPut("{studentInfoId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateStudentInfo")]
    [EndpointSummary("Updates studentInfo")]
    [EndpointDescription("Updates studentInfo")]
    public async Task<ActionResult> UpdateStudentInfo(Guid studentInfoId,UpdateStudentInfoRequest request, CancellationToken ct = default) 
    {
        await studentInfoService.UpdateStudentInfo(studentInfoId,request, ct);

        return NoContent();
    } 

    [HttpGet("{studentInfoId}")]
    [Consumes("application/json")]
    [ProducesResponseType<StudentInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetStudentInfoById")]
    [EndpointSummary("Get studentInfo by id")]
    [EndpointDescription("Get studentInfo by id")]
    public async Task<ActionResult<StudentInfoResponse>> GetStudentInfoById(Guid studentInfoId, CancellationToken ct = default) 
    {
        var studentInfoResponse = await studentInfoService.GetStudentInfoById(studentInfoId, ct);

        return Ok(studentInfoResponse);
    }
    
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<List<StudentInfoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetStudentInfoPage")]
    [EndpointSummary("Get a page of studentInfos")]
    [EndpointDescription("Get the page studentInfos")]
    public async Task<ActionResult<PagedResult<StudentInfoResponse>>> GetStudentInfoPage([FromQuery]StudentInfoFilter? filter,CancellationToken ct = default) 
    {
        var studentInfosPageResponse = await studentInfoService.GetPagedStudentInfos(filter,ct);

        return Ok(studentInfosPageResponse);
    }

    [HttpDelete("{studentInfoId}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("DeleteStudentInfo")]
    [EndpointSummary("Delete studentInfo")]
    [EndpointDescription("Delete studentInfo.")]
    public async Task<ActionResult> DeleteStudentInfo(Guid studentInfoId, CancellationToken ct = default) 
    {
        await studentInfoService.DeleteStudentInfo(studentInfoId, ct);

        return NoContent();
    }
}
