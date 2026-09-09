using Ces_Platform_Server_Side.Requests;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ces_Platform_Server_Side.Models;

public class Note : AuditableEntity
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }

    // public IFormFile File { get; set; }
    //fks
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    //Navigation
    public Teacher Teacher { get; set; } = null!;
    public Course Course { get; set; } = null!;

    public Note(DateOnly date, string name, Guid courseId, Guid teacherId, string createdBy)
    : base(createdBy)
    {
        Date = date;
        Name = name;
        CourseId = courseId;
        TeacherId = teacherId;
        //File = file;
    }

    public static Note Create(CreateNoteRequest requset, string CreatedBy)
    {
        return new Note(DateOnly.FromDateTime(DateTime.Now), requset.NoteName, requset.CourseId, requset.TeacherId, CreatedBy);
    }

    public void Assign(UpdateNoteRequest request, string LastModifyBy)
    {
        Name = request.NoteName;
        CourseId = request.CourseId;
        TeacherId = request.TeacherId;
        LastModifiedBy = LastModifyBy;
        LastModifiedAtUtc = DateTimeOffset.UtcNow;
    }

    public bool IsEqual(UpdateNoteRequest request)
    {
        return Name == request.NoteName;
    }

}
