namespace StudentsAssociation.Models;

public class Course:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    // Navigation
    List<Test> Tests = [];
    List<Note> Notes = [];

    public Course(string name)
    {
        Name = name;
    }
}
