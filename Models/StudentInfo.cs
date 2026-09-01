namespace Ces_Platform_Server_Side.Models;

public class StudentInfo:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;

    // Navigation Properties
    
    // public List<string> Skills { get; set; } = [];
    // public Dictionary<string,string> Sources { get; set; } = [];
    public StudentInfo()
    {}
    public StudentInfo(string name, string about, string major)
    {
        Name = name;
        About = about;
        Major = major;
    }
}
