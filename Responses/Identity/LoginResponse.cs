using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Responses;

public class LoginResponse
{
    public TokenResponse Token { get; set; } = new();
    public UserRole Role { get; set; } 
}
