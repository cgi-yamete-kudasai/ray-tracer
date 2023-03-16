using CommandLine;

namespace RayTracer.Sample.CommandArgsParser;

public class CommandLineOptions
{
    [Option('s', "source", Required = true, HelpText = "Set path to the input file")]
    public string SourcePath { get; set; }

    [Option("goal-format", Required = false, HelpText = "Set goal format")]
    public string GoalFormat { get; set; }

    [Option('o', "output", Required = false, HelpText = "Set path to the output file")]
    public string OutputPath { get; set; }
}