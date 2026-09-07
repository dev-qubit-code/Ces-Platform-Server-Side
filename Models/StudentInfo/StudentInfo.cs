namespace Ces_Platform_Server_Side.Models;

public class StudentInfo:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;

    // Navigation Properties
    public List<Skill> Skills { get; set; } = [];
    public List<StudentInfoSkill> StudentInfoSkills { get; set; } = [];
    public List<Source> Sources { get; set; } = [];
    public StudentInfo()
    {}
    public StudentInfo(string name, string about, string major, List<Skill> skills, List<Source> sources)
    {
        Name = name;
        About = about;
        Major = major;
        Skills = skills;
        Sources = sources;
    }
}
