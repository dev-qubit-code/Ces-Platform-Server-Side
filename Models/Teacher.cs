using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Models;

public class Teacher:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    // navigation 
    // List<Test> Tests = [];
    // List<Note> Notes = [];
    public Teacher(string name)
    {
        Name = name;
    }

     public static Teacher Create(CreateTeacherRequest request, string createdBy) => new Teacher(createdBy)
    {
            Name = request.Name
    };

    public void Assign(UpdateTeacherRequest request, string lastModifiedBy)
    {
        Name = request.Name;
        LastModifiedAtUtc = DateTimeOffset.UtcNow;
        LastModifiedBy = lastModifiedBy;
    } 

    public bool IsEqual(UpdateTeacherRequest request)
    {
        if(Name == request.Name)
            return true;

        return false;
    } 
}
