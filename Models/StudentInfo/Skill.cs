namespace Ces_Platform_Server_Side.Models;

public class Skill : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    // navigation properties
    public List<StudentInfo> StudentInfos { get; set; } = [];
}