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
        return ToCommandLineToolOptions(options, tool, [singleArgument]);
    }

    public static CommandLineToolOptions ToCommandLineToolOptions(this CommandLineOptions options, string tool, string[] arguments)
    {
        return new CommandLineToolOptions(tool)
        {
            Arguments = arguments,
            CommandLineCredentials = options.CommandLineCredentials,
            EnvironmentVariables = options.EnvironmentVariables,
            CommandLogging = options.CommandLogging,
            WorkingDirectory = options.WorkingDirectory,
            OptionsObject = options,
        };
    }

    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, string singleArgument)
    {
        return WithArguments(options, [singleArgument]);
    }

    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, IEnumerable<string>? arguments)
    {
        return WithArguments(options, arguments?.ToArray() ?? []);
    }

    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, params string[] arguments)
    {
        return options with
        {
            Arguments = (options.Arguments ?? Array.Empty<string>()).Concat(arguments),
        };
    }
}