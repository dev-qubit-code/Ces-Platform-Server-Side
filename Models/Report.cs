using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Models;

public class Report:AuditableEntity
{

    public string TItle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Report(string tItle, string description, string createdBy) : base(createdBy)
    {
        TItle = tItle;
        Description = description;
    }

    public static Report Create(CreateReportRequest request, string createdBy) => new Report(
        request.Title,
        request.Description,
        createdBy
        );
}
