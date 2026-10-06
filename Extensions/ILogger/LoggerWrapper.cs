using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Extensions;

public class LoggerWrapper<T>(ILogger<T> logger, ILogRepository logsRepository) : ILoggerWrapper<T>
{
    public void LogTrace(string message, UserRole role)
    {
        logger.LogTrace(message);

        AddLog(message, role);
    }
    public void LogDebug(string message, UserRole role)
    {
        logger.LogDebug(message);
        
        AddLog(message, role);
    }

    public void LogInformation(string message, UserRole role)
    {
        logger.LogInformation(message);
        
        AddLog(message, role);
    }   

    public void LogWarning(string message, UserRole role)
    {
        logger.LogWarning(message);
        
        AddLog(message, role);
    }   
    public void LogError(string message, UserRole role)
    {
        logger.LogError(message);
        
        AddLog(message, role);
    }   
    
    public void LogCritical(string message, UserRole role)
    {
        logger.LogCritical(message);
        
        AddLog(message, role);
    }   

    private void AddLog(string message, UserRole role)
    {
        if (!logsRepository.AddLog(Log.Create(message, role)))
            throw new InvalidOperationException("Error occured while adding the logs");
    }

}