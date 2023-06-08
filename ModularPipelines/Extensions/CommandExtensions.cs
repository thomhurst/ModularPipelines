using ModularPipelines.Options;

namespace ModularPipelines.Extensions;

public static class CommandExtensions
{
    public static CommandLineToolOptions ToCommandLineToolOptions(this CommandEnvironmentOptions options, string tool, IEnumerable<string> arguments)
    {
        return new CommandLineToolOptions(tool)
        {
            Arguments = arguments,
            Credentials = options.Credentials,
            EnvironmentVariables = options.EnvironmentVariables,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput,
            WorkingDirectory = options.WorkingDirectory
        };
    }
}