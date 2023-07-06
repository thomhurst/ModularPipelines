using ModularPipelines.Options;

namespace ModularPipelines.Extensions;

public static class CommandExtensions
{
    public static CommandLineToolOptions ToCommandLineToolOptions(this CommandLineOptions options, string tool,
        IEnumerable<string> arguments)
    {
        return ToCommandLineToolOptions(options, tool, arguments.ToArray());
    }

    public static CommandLineToolOptions ToCommandLineToolOptions(this CommandLineOptions options, string tool,
        string singleArgument)
    {
        return ToCommandLineToolOptions(options, tool, new[] { singleArgument });
    }

    public static CommandLineToolOptions ToCommandLineToolOptions(this CommandLineOptions options, string tool, string[] arguments)
    {
        return new CommandLineToolOptions(tool)
        {
            Arguments = arguments,
            Credentials = options.Credentials,
            EnvironmentVariables = options.EnvironmentVariables,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput,
            WorkingDirectory = options.WorkingDirectory,
            ArgumentsOptionObject = options
        };
    }

    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, string singleArgument)
    {
        return WithArguments(options, new[] { singleArgument });
    }

    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, IEnumerable<string>? arguments)
    {
        return WithArguments(options, arguments?.ToArray() ?? Array.Empty<string>());
    }

    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, params string[] arguments)
    {
        return new CommandLineToolOptions(options.Tool)
        {
            Arguments = arguments.Concat(options.Arguments ?? Array.Empty<string>()),
            Credentials = options.Credentials,
            EnvironmentVariables = options.EnvironmentVariables,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput,
            WorkingDirectory = options.WorkingDirectory,
            ArgumentsOptionObject = options.ArgumentsOptionObject ?? options,
            AdditionalSwitches = options.AdditionalSwitches,
        };
    }
}
