using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;
public class LogPageResponse
{
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public static LogPageResponse FromModel(Log log) => new()
    {
        Message = log.Message,
        CreatedAtUtc = log.CreatedAtUtc,
    };

    public static IEnumerable<LogPageResponse> FromModels(IEnumerable<Log> logs) => logs.Select(FromModel);
}
