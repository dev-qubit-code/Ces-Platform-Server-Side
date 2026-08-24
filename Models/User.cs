using Microsoft.AspNetCore.Http.HttpResults;
using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Models;
public class User:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    public User()
    {
        
    }
    public User(string createdBy) : base(createdBy)
    {
    }

    public static User Create(CreateUserRequest request, string createdBy) => new User(createdBy)
    {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Role = request.Role,
    };

    public void Assign(UpdateUserRequest request, string lastModifiedBy)
    {
        Name = request.Name;
        Email = request.Email;
        Password = request.Password;
        Role = request.Role;
        IsActive = request.IsActive;
        LastModifiedAtUtc = DateTimeOffset.UtcNow;
        LastModifiedBy = lastModifiedBy;
    } 

    public bool IsEqual(UpdateUserRequest request)
    {
        if(
            Name == request.Name &&
            Email == request.Email &&
            Password == request.Password &&
            Role == request.Role &&
            IsActive == request.IsActive
        )
            return true;

        return false;
    } 
}
