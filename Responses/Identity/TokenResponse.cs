namespace Ces_Platform_Server_Side.Responses;

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
