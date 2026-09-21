using Asp.Versioning;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ces_Platform_Server_Side.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/teachers")]
[ApiVersion("1.0")]
[Tags("Teachers")]
public class TeacherController(ITeacherService teacherService) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<TeacherResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("CreateTeacher")]
    [EndpointSummary("Create teacher")]
    [EndpointDescription("Create teacher")]
    public async Task<ActionResult<TeacherResponse>> CreateTeacher(CreateTeacherRequest request, CancellationToken ct = default) 
    {
        var teacherResponse = await teacherService.CreateTeacher(request, ct);

        return CreatedAtAction(nameof(GetTeacherById), new { teacherId = teacherResponse.Id }, teacherResponse);
    } 

    [HttpPut("{teacherId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateTeacher")]
    [EndpointSummary("Updates teacher")]
    [EndpointDescription("Updates teacher")]
    public async Task<ActionResult> UpdateTeacher(Guid teacherId,UpdateTeacherRequest request, CancellationToken ct = default) 
    {
        await teacherService.UpdateTeacher(teacherId,request, ct);

        return NoContent();
    } 

    [HttpGet("{teacherId}")]
    [Consumes("application/json")]
    [ProducesResponseType<TeacherResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetTeacherById")]
    [EndpointSummary("Get teacher by id")]
    [EndpointDescription("Get teacher by id")]
    public async Task<ActionResult<TeacherResponse>> GetTeacherById(Guid teacherId, CancellationToken ct = default) 
    {
        var teacherResponse = await teacherService.GetTeacherById(teacherId, ct);

        return Ok(teacherResponse);
    }
    
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<List<TeacherResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetTeacherPage")]
    [EndpointSummary("Get a page of teachers")]
    [EndpointDescription("Get the page teachers")]
    public async Task<ActionResult<PagedResult<TeacherResponse>>> GetTeacherPage([FromQuery]TeacherFilter? filter,CancellationToken ct = default) 
    {
        var teachersPageResponse = await teacherService.GetPagedTeachers(filter,ct);

        return Ok(teachersPageResponse);
    }

    [HttpDelete("{teacherId}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("DeleteTeacher")]
    [EndpointSummary("Delete teacher")]
    [EndpointDescription("Delete teacher.")]
    public async Task<ActionResult> DeleteTeacher(Guid teacherId, CancellationToken ct = default) 
    {
        await teacherService.DeleteTeacher(teacherId, ct);

        return NoContent();
    }
}
