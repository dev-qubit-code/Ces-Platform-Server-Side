using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Extensions;

public class LoggerWrapper<T>(ILogger<T> logger, ILogRepository logsRepository) : ILoggerWrapper<T>
{
    public void LogInformation(string message, UserRole role)
    {
        logger.LogInformation(message);

        // + add to history table
        
        if(!logsRepository.AddLog(Log.Create(message, role)))
            throw new InvalidOperationException("Error occured while adding the logs");
    }   


}