namespace RayTracer.Library.Diagnostics.Logging;

public interface ILog
{
    void WriteLine(string message, LogSeverity severity);
}
