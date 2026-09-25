
using Asp.Versioning;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/courses")]
[ApiVersion("1.0")]
[Tags("course")]
public class CourseControllers(ICourseService service) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("CreateCourse")]
    [EndpointSummary("Create a new Course")]
    [EndpointDescription("Create a new course using Endpoint")]

    public async Task<ActionResult<CourseResponse>> CreateCourse(CreateCourseRequest request, CancellationToken ct = default)
    {
        var respons = await service.CreateCourse(request, ct);

        return CreatedAtAction(nameof(GetCourseById), new { Id = respons.Id }, respons);
    }

    [HttpGet("{Id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetCourseById")]
    [EndpointSummary("Get course Using Id")]
    [EndpointDescription("Get course From DataBase using Id")]

    public async Task<ActionResult<CourseResponse>> GetCourseById(Guid Id, CancellationToken ct = default)
    {
        var Course = await service.GetCourseById(Id, ct);
        return Ok(Course);
    }

    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<List<CourseResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetCoursePage")]
    [EndpointSummary("Get a page of Course")]
    [EndpointDescription("Get a Page From Course By Query String")]

    public async Task<ActionResult<PagedResult<CoursePageResponse>>> GetCoursePage([FromQuery] CourseFilter? filter, CancellationToken ct = default)
    {
        PagedResult<CoursePageResponse> pageResult = await service.GetPagedCourses(filter, ct);
        return pageResult;
    }


    [HttpDelete("{Id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("DeleteCourseById")]
    [EndpointSummary("Delete a course Using Id")]
    [EndpointDescription("Delete a course By Id From Database")]

    public async Task<ActionResult> DeleteCourseById(Guid Id,CancellationToken ct = default)
    {
        await service.DeleteCourse(Id);
        return NoContent();
    }

    [HttpPut("{Id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateCourse")]
    [EndpointSummary("Update course Using Id")]
    [EndpointDescription("Update course Fields Using Id")]

    public async Task<ActionResult> UpdateCourse(Guid Id,UpdateCourseRequest request,CancellationToken ct = default)
    {
        await service.UpdateCourse(Id, request, ct);
        return NoContent();
    }


}