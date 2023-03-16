using System.IO;

namespace RayTracer.Sample.CommandArgsParser;

public static class CommandArgsParser
{
    public static bool ParseSourcePath(string fileName, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (!File.Exists(fileName))
        {
            errorMessage = "Path to the source file is invalid";
            return false;
        }

        return true;
    }
}