namespace Ces_Platform_Server_Side.Models;

public class Report:AuditableEntity
{

    public string TItle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Report(string tItle, string description)
    {
        TItle = tItle;
        Description = description;
    }
}
