
using Asp.Versioning;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Requests.Course;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Responses.Course;
using Ces_Platform_Server_Side.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[ApiController]
[Route("api/v{version:apiVersion}/course")]
[ApiVersion("1.0")]
[Tags("course")]
public class CourseControllers(CourseService service) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Create Course")]
    [EndpointSummary("Create course")]
    [EndpointDescription("Create course")]

    public async Task<ActionResult<CourseResponse>> Create(CreateCourseRequest request, CancellationToken ct = default)
    {
        var respons = await service.CreateCourse(request, ct);

        return CreatedAtAction(nameof(GetById), new { Id = respons.Id }, respons);
    }

    [HttpGet("{Id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Get Course")]
    [EndpointSummary("Get course")]
    [EndpointDescription("Get course")]

    public async Task<ActionResult<CourseResponse>> GetById(Guid Id, CancellationToken ct = default)
    {
        var Course = await service.GetCourseById(Id, ct);
        return Ok(Course);
    }

    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Get Course Page")]
    [EndpointSummary("Get a page of Course")]
    [EndpointDescription("Get the page Courses")]

    public async Task<ActionResult<PagedResult<CoursePageResponse>>> GetPage([FromQuery] CourseFilter? filter, CancellationToken ct = default)
    {
        PagedResult<CoursePageResponse> pageResult = await service.GetPagedCourses(filter, ct);
        return pageResult;
    }


    [HttpDelete("{Id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("DELETE Course")]
    [EndpointSummary("Delete course")]
    [EndpointDescription("Delete course")]

    public async Task<ActionResult> DeleteById(Guid Id,CancellationToken ct = default)
    {
        await service.DeleteCourse(Id);
        return NoContent();
    }

    [HttpPut("{Id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Update Course")]
    [EndpointSummary("Update course")]
    [EndpointDescription("Update course")]

    public async Task<ActionResult> Update(Guid Id,UpdateCourseRequest request,CancellationToken ct = default)
    {
        await service.UpdateCourse(Id, request, ct);
        return NoContent();
    }


}