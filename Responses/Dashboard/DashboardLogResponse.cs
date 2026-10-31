using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class DashboardLogResponse
{
   public string Message { get; set; } = string.Empty;
   public DateTimeOffset LogDateTime { get; set; }

   public static DashboardLogResponse FromModel(Log log) => new()
   {
        Message = log.Message,
        LogDateTime = log.CreatedAtUtc
   };

    public static IEnumerable<DashboardLogResponse> FromModels(IEnumerable<Log> logs) => logs.Select(FromModel);
}