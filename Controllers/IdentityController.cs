using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Services;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
[Tags("Identites")]
public class IdentityController(IdentityService identityService) : ControllerBase
{
    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Login")]
    [EndpointSummary("login for user")]
    [EndpointDescription("login for user accepting email and password and returning access token only")]
    public async Task<LoginResponse> Login(GenerateTokenRequest request, CancellationToken ct)
    {
        var obj = await identityService.Login(request, ct);
        return obj;
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType<MeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("Me")]
    [EndpointSummary("Decrypt the information of the user from the token")]
    [EndpointDescription("Decrypt the information of the user from the token to show him the data")]
    public async Task<ActionResult<MeResponse>> Me(CancellationToken ct = default) 
    {
        return new MeResponse()
        { 
          Id = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
          Email = User.FindFirstValue(ClaimTypes.Email)!,
          Name = User.FindFirstValue(ClaimTypes.GivenName)!,
          Role = Enum.Parse<UserRole>(User.FindFirstValue(ClaimTypes.Role)!),
          IsActive = bool.Parse(User.FindFirstValue("Activation")!)
        };
    }
    
}