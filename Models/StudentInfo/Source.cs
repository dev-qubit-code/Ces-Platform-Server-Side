namespace Ces_Platform_Server_Side.Models;

public class Source : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    // fks
    public Guid StudentInfoId { get; set; }
    // navigation properties
    public StudentInfo StudentInfo { get; set; } = null!;
}
