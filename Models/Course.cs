using Ces_Platform_Server_Side.Requests.Course;
using Microsoft.EntityFrameworkCore;

namespace Ces_Platform_Server_Side.Models;

public class Course : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    // Navigation

    //public List<Test> Tests { set; get; } = [];
    //public List<Note> Notes { set; get; } = [];

    public Course(string name)
    {
        Name = name;
    }
    public Course(string name, string CreatedBy) : base(CreatedBy)
    {
        Name = name;
    }

    public static Course Create(CreateCourseRequest request, string createdBy)
    {
        Course course = new Course(request.Name, createdBy);
        return course;
    }

    public void Assign(UpdateCourseRequest request, string LastModifyBy)
    {
        Name = request.Name;

        LastModifiedBy = LastModifyBy;
        LastModifiedAtUtc = DateTimeOffset.UtcNow;
    }

    public bool IsEqual(UpdateCourseRequest? obj)
    {
        return Name == obj.Name; 
    }

}
