using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Interfaces;

public interface ILoggerWrapper<T>
{
    public void LogTrace(string message, UserRole role);
    public void LogDebug(string message, UserRole role);
    public void LogInformation(string message, UserRole role);
    public void LogWarning(string message, UserRole role);
    public void LogError(string message, UserRole role);
    public void LogCritical(string message, UserRole role);
}