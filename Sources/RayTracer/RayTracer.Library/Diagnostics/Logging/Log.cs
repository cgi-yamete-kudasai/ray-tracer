namespace RayTracer.Library.Diagnostics.Logging;

public static class Log
{
    public static ILog Default => ConsoleLog.Instance;
}
