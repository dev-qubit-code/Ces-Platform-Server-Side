using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Interfaces;

public interface ILoggerWrapper<T>
{
    public void LogInformation(string message, UserRole role);
}