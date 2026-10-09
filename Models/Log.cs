using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Models;
public class Log : Entity
{
    public string Message { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Log(string message, UserRole role)
    {
        Message = message;
        Role = role;
    }

    public static Log Create(string message, UserRole role) => new(message, role);
}
