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
}
