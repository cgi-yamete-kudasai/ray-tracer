using RayTracer.Library.Diagnostics.Logging;

namespace RayTracer.Library.Extensions;

public static class LogExtensions
{
    public static void Info(this ILog log, string message) => log.WriteLine(message, LogSeverity.Info);

    public static void Warning(this ILog log, string message) => log.WriteLine(message, LogSeverity.Warning);
    
    public static void Error(this ILog log, string message) => log.WriteLine(message, LogSeverity.Error);
}
