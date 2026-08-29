using Asp.Versioning;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
[Tags("Users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Create")]
    [EndpointSummary("Create user")]
    [EndpointDescription("Create user")]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request, CancellationToken ct = default) 
    {
        var userResponse = await userService.CreateUser(request, ct);

        return CreatedAtAction(nameof(GetById), new { userId = userResponse.Id }, userResponse);
    } 

    [HttpPut("{userId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Update")]
    [EndpointSummary("Updates user")]
    [EndpointDescription("Updates user")]
    public async Task<ActionResult> Update(Guid userId,UpdateUserRequest request, CancellationToken ct = default) 
    {
        await userService.UpdateUser(userId,request, ct);

        return NoContent();
    } 

    [HttpGet("{userId}")]
    [Consumes("application/json")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetById")]
    [EndpointSummary("Get user by id")]
    [EndpointDescription("Get user by id")]
    public async Task<ActionResult<UserResponse>> GetById(Guid userId, CancellationToken ct = default) 
    {
        var userResponse = await userService.GetUserById(userId, ct);

        return Ok(userResponse);
    }
    
    [HttpGet]
    [Consumes("application/json")]
    [ProducesResponseType<List<UserResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetPage")]
    [EndpointSummary("Get a page of users")]
    [EndpointDescription("Get the page users")]
    public async Task<ActionResult<PagedResult<UserResponse>>> GetPage([FromQuery]UserFilter? filter,CancellationToken ct = default) 
    {
        var usersPageResponse = await userService.GetPagedUsers(filter,ct);

        return Ok(usersPageResponse);
    }

    [HttpDelete("{userId}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Delete")]
    [EndpointSummary("Delete user")]
    [EndpointDescription("Delete user.")]
    public async Task<ActionResult> Delete(Guid userId, CancellationToken ct = default) 
    {
        await userService.DeleteUser(userId, ct);

        return NoContent();
    }

    [HttpPut("{userId:guid}/activation")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateUserActivation")]
    [EndpointSummary("User Activation")]
    [EndpointDescription("User Activation.")]
    public async Task<ActionResult> UpdateUserActivation(Guid userId,UpdateUserActivationRequest request, CancellationToken ct = default) 
    {
        await userService.UpdateUserActivation(userId,request, ct);

        return NoContent();
    }
}