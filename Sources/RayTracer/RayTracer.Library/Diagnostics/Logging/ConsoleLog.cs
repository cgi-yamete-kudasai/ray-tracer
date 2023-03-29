using System;
using System.Diagnostics;
using RayTracer.Library.Utils;

namespace RayTracer.Library.Diagnostics.Logging;

public class ConsoleLog : Singleton<ConsoleLog>, ILog
{
    public void WriteLine(string message, LogSeverity severity)
    {
        Console.ForegroundColor = GetSeverityColor(severity);

        string prefix = GetSeverityPrefix(severity);
        Console.WriteLine($"[{prefix}]: {message}", severity);

        Console.ForegroundColor = ConsoleColor.Gray;
    }

    private static ConsoleColor GetSeverityColor(LogSeverity severity) => severity switch
    {
        LogSeverity.Info => ConsoleColor.Gray,
        LogSeverity.Warning => ConsoleColor.Yellow,
        LogSeverity.Error => ConsoleColor.Red,
        _ => throw new UnreachableException()
    };

    private static string GetSeverityPrefix(LogSeverity severity) => severity switch
    {
        LogSeverity.Info => "INFO",
        LogSeverity.Warning => "WARNING",
        LogSeverity.Error => "ERROR",
        _ => throw new UnreachableException()
    };
}
