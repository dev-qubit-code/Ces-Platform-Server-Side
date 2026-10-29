using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class DashboardLogResponse
{
   public string Message { get; set; } = string.Empty;
   public DateTime LogDateTime { get; set; }

   public static DashboardLogResponse FromModel(Log log) => new()
   {
        Message = log.Message,
        LogDateTime = DateTime.Now
   };

    public static IEnumerable<DashboardLogResponse> FromModels(IEnumerable<Log> logs) => logs.Select(FromModel);
}