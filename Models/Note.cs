using Ces_Platform_Server_Side.Requests;
namespace Ces_Platform_Server_Side.Models;

public class Note:AuditableEntity
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public IFormFile File { get; set; }
    //fks
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    //Navigation
    public Teacher Teacher { get; set; } = null!;
    public Course Course { get; set; } = null!;

    public Note(DateOnly date, IFormFile file, string name)
    {
        Date = date;
        File = file;
        Name = name;
    }
    public Note(DateOnly date, IFormFile file, string name, string CreatedBy) : base(CreatedBy) => new Note(date, file, name);
    public static Note Create(CreateNoteRequest requset,string CreatedBy)
    {
        return new Note(DateOnly.FromDateTime(DateTime.Now), requset.NoteFile, requset.NoteName,CreatedBy);
    }

    public void Assign(UpdateNoteRequest request,string LastModifyBy)
    {
        LastModifiedBy = LastModifyBy;
        LastModifiedAtUtc = DateTimeOffset.UtcNow;
        Name = request.NoteName;
    }

    public bool IsEqual(UpdateNoteRequest request)
    {
        return Name == request.NoteName;
    }

}
