
using Asp.Versioning;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Ces_Platform_Server_Side.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tests")]
    [ApiVersion("1.0")]
    [Tags("tests")]
    public class TestControllers(ITestService service):ControllerBase
    {
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType<TeacherResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("CreateTest")]
        [EndpointSummary("Create test")]
        [EndpointDescription("Create test")]
        [Authorize(Policy = "Manager/Admin")]

        public async Task<ActionResult<TestResponse>> Createtest([FromBody] CreateTestRequest request, CancellationToken ct = default)
        {
            var testResponse = await service.CreateTest(request, ct);

            return CreatedAtAction(nameof(GettestById), new { testId = testResponse.Id }, testResponse);
        }

        [HttpPut("{testId:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("Update Test")]
        [EndpointSummary("Update Test")]
        [EndpointDescription("Update Test")]
        [Authorize(Policy = "Manager/Admin")]

        public async Task<ActionResult> UpdateTest(Guid testId, UpdateTestRequest request, CancellationToken ct = default)
        {
            await service.UpdateTest(testId, request, ct);

            return NoContent();
        }

        [HttpGet("{testId}")]
        [Consumes("application/json")]
        [ProducesResponseType<TestResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("GetTestById")]
        [EndpointSummary("Get test by id")]
        [EndpointDescription("Get test by id")]
        public async Task<ActionResult<TestResponse>> GettestById(Guid testId, CancellationToken ct = default)
        {
            var testResponse = await service.GetTestById(testId, ct);

            return Ok(testResponse);
        }

        [HttpGet]
        [Consumes("application/json")]
        [ProducesResponseType<List<TestResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("GetTestPage")]
        [EndpointSummary("Get a page of tests")]
        [EndpointDescription("Get the page tests")]
        public async Task<ActionResult<PagedResult<TestResponse>>> GettestPage([FromQuery] TestFilter? filter, CancellationToken ct = default)
        {
            var testsPageResponse = await service.GetPagedTests(filter, ct);

            return Ok(testsPageResponse);
        }

        [HttpDelete("{testId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("DeleteTest")]
        [EndpointSummary("Delete test")]
        [EndpointDescription("Delete test.")]
        [Authorize(Policy = "Manager/Admin")]

        public async Task<ActionResult> Deletetest(Guid testId, CancellationToken ct = default)
        {
            await service.DeleteTest(testId, ct);

            return NoContent();
        }
    }
}
