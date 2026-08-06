using StudentsAssociation.Enums;

namespace StudentsAssociation.Models;

public class Test:AuditableEntity
{

    public TestStatus Status { get; set; }
    public IFormFile? File { get; set; }
    public DateOnly Date { get; set; }
    public TestKind Kind { get; set; }
    //fks
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    // Navigation
    public Teacher Teacher { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public Test(TestStatus status, IFormFile? file, DateOnly date, TestKind kind)
    {
        Status = status;
        File = file;
        Date = date;
        Kind = kind;
    }
}
