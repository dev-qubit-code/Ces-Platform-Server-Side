using Asp.Versioning;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ces_Platform_Server_Side.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/notes")]
    [ApiVersion("1.0")]
    [Tags("Notes")]
    public class NoteController(INoteService service) : ControllerBase
    {
        [HttpGet("{Id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType<TeacherResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("NoteById")]
        [EndpointSummary("Get Note Using Id")]
        public async Task<ActionResult<NoteResponse>> GetNoteById(Guid Id, CancellationToken ct = default) => Ok(await service.GetNoteById(Id, ct));

        [HttpPut("{Id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType<TeacherResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("UpdateNote")]
        [EndpointSummary("Update Note Using Id")]
        public async Task<ActionResult> UpdateNote(Guid Id, [FromBody] UpdateNoteRequest request, CancellationToken ct = default)
        {
            await service.UpdateNote(Id, request, ct);
            return NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType<TeacherResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("CreateNote")]
        [EndpointSummary("Post a Note")]
        public async Task<ActionResult> CreateNote(CreateNoteRequest request, CancellationToken ct = default)
        {
            NoteResponse response = await service.CreateNote(request, ct);
            return CreatedAtAction(nameof(GetNoteById), new { Id = response.Id }, response);
        }

        [HttpDelete("{Id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType<TeacherResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("DeleteNote")]
        [EndpointSummary("Delete a Note Using Id")]
        public async Task<ActionResult> DeleteNote(Guid Id, CancellationToken ct = default)
        {
            await service.DeleteNote(Id, ct);
            return NoContent();
        }

        [HttpGet]
        [Consumes("application/json")]
        [ProducesResponseType<TeacherResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("GetNotesPage")]
        [EndpointSummary("Get Page Of Notes Using Filter")]
        public async Task<ActionResult> GetNotesPage([FromQuery] NoteFilter? filter, CancellationToken ct = default)
        {
            PagedResult<NotePageResponse> Result = await service.GetPagedNotes(filter, ct);
            return Ok(Result);
        }


    }

}
