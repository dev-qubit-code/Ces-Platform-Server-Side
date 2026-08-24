using Azure;
using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;
public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTimeOffset JoinDate { get; set; } 
    public bool IsActive { get; set; }

    public static UserResponse FromModel(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role,
        JoinDate = user.CreatedAtUtc,
        IsActive = user.IsActive,
    };

    public static IEnumerable<UserResponse> FromModel(IEnumerable<User> users) => users.Select(FromModel);
}