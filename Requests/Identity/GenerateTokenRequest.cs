namespace Ces_Platform_Server_Side.Requests;

public class GenerateTokenRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
