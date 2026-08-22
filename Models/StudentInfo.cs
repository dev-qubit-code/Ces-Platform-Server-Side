namespace Ces_Platform_Server_Side.Models;

public class StudentInfo:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
    public Dictionary<string,string> Sources { get; set; } = [];
    public StudentInfo(string name, string about, List<string> skills, Dictionary<string,string> sources)
    {
        Name = name;
        About = about;
        Skills = skills;
        Sources = sources;
    }
}
