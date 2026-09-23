using Ces_Platform_Server_Side.Enums;

namespace Ces_Platform_Server_Side.Requests;
public class CreateReportRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ReportPriority Priority { get; set; }
}
