using StudentsAssociation.Enums;

namespace StudentsAssociation.Models;
public class User:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool Active { get; set; } = true;

    public User(string name, string email, string password, UserRole role, string createdBy) : base(createdBy)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }
}
