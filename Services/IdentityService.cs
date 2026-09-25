
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Ces_Platform_Server_Side.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Repositories;
using Ces_Platform_Server_Side.Requests;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.Interfaces;

namespace Ces_Platform_Server_Side.Services;
public class IdentityService(IConfiguration configuration,IUserRepository UserRepository)
{
    public async Task<LoginResponse> Login(GenerateTokenRequest request,CancellationToken ct)
    {
        var user = await UserRepository.GetUserByEmailAsync(request.Email,ct)??
            throw new BusinessRuleException("Invalid Email or Password",StatusCodes.Status409Conflict);

        if(user.Password != request.Password)
            throw new BusinessRuleException("Invalid Email or Password",StatusCodes.Status409Conflict);

        return new (){ Token = GenerateJwtToken(user) , Role = user.Role};

    }

    public TokenResponse GenerateJwtToken(User user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var key = jwtSettings["SecretKey"];
        var expiry = DateTime.UtcNow.AddDays(int.Parse(jwtSettings["TokenExpirationInDays"]!));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,user.Id.ToString()),  
            new(JwtRegisteredClaimNames.GivenName,user.Name),  
            new(ClaimTypes.Email,user.Email),  
            new(ClaimTypes.Role,user.Role.ToString()),
            new("Activation",user.IsActive.ToString())
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = issuer,
            Audience = audience,
            Expires = expiry,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(descriptor);

        return new TokenResponse { AccessToken = tokenHandler.WriteToken(securityToken),Expires = expiry};
    }
}
