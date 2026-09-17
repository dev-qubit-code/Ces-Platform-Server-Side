using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class ReportPageResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public static ReportPageResponse FromModel(Report report) => new()
    {
        Id = report.Id,
        Title = report.TItle,
    };

    public static IEnumerable<ReportPageResponse> FromModels(IEnumerable<Report> reports) => reports.Select(FromModel);
}