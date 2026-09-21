using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class ReportPageResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ReportPriority Priority { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public static ReportPageResponse FromModel(Report report) => new()
    {
        Id = report.Id,
        Title = report.TItle,
        Description = report.Description,
        Priority = report.Priority,
        CreatedAtUtc = report.CreatedAtUtc
    };

    public static IEnumerable<ReportPageResponse> FromModels(IEnumerable<Report> reports) => reports.Select(FromModel);
}