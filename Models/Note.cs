namespace StudentsAssociation.Models;

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

}
