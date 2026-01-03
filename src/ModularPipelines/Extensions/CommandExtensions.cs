using ModularPipelines.Options;

namespace ModularPipelines.Extensions;

/// <summary>
/// Provides extension methods for working with command-line options.
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Converts <see cref="CommandExecutionOptions"/> to <see cref="GenericCommandLineToolOptions"/> with the specified tool and arguments.
    /// </summary>
    /// <param name="options">The command execution options to convert.</param>
    /// <param name="tool">The name or path of the command-line tool.</param>
    /// <param name="arguments">The arguments to pass to the tool.</param>
    /// <returns>A new <see cref="GenericCommandLineToolOptions"/> instance configured with the specified tool and arguments.</returns>
    public static GenericCommandLineToolOptions ToCommandLineToolOptions(this CommandExecutionOptions options, string tool,
        IEnumerable<string> arguments)
    {
        return ToCommandLineToolOptions(options, tool, arguments.ToArray());
    }

    /// <summary>
    /// Converts <see cref="CommandExecutionOptions"/> to <see cref="GenericCommandLineToolOptions"/> with the specified tool and a single argument.
    /// </summary>
    /// <param name="options">The command execution options to convert.</param>
    /// <param name="tool">The name or path of the command-line tool.</param>
    /// <param name="singleArgument">A single argument to pass to the tool.</param>
    /// <returns>A new <see cref="GenericCommandLineToolOptions"/> instance configured with the specified tool and argument.</returns>
    public static GenericCommandLineToolOptions ToCommandLineToolOptions(this CommandExecutionOptions options, string tool,
        string singleArgument)
    {
        return ToCommandLineToolOptions(options, tool, [singleArgument]);
    }

    /// <summary>
    /// Converts <see cref="CommandExecutionOptions"/> to <see cref="GenericCommandLineToolOptions"/> with the specified tool and arguments array.
    /// </summary>
    /// <param name="options">The command execution options to convert.</param>
    /// <param name="tool">The name or path of the command-line tool.</param>
    /// <param name="arguments">An array of arguments to pass to the tool.</param>
    /// <returns>A new <see cref="GenericCommandLineToolOptions"/> instance configured with the specified tool and arguments.</returns>
    public static GenericCommandLineToolOptions ToCommandLineToolOptions(this CommandExecutionOptions options, string tool, string[] arguments)
    {
        return new GenericCommandLineToolOptions(tool)
        {
            Arguments = arguments,
        };
    }

    /// <summary>
    /// Creates a new <see cref="CommandLineToolOptions"/> with a single argument appended to the existing arguments.
    /// </summary>
    /// <param name="options">The command-line tool options to extend.</param>
    /// <param name="singleArgument">A single argument to append.</param>
    /// <returns>A new <see cref="CommandLineToolOptions"/> instance with the argument appended.</returns>
    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, string singleArgument)
    {
        return WithArguments(options, [singleArgument]);
    }

    /// <summary>
    /// Creates a new <see cref="CommandLineToolOptions"/> with additional arguments appended to the existing arguments.
    /// </summary>
    /// <param name="options">The command-line tool options to extend.</param>
    /// <param name="arguments">The arguments to append, or null to append nothing.</param>
    /// <returns>A new <see cref="CommandLineToolOptions"/> instance with the arguments appended.</returns>
    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, IEnumerable<string>? arguments)
    {
        return WithArguments(options, arguments?.ToArray() ?? []);
    }

    /// <summary>
    /// Creates a new <see cref="CommandLineToolOptions"/> with additional arguments appended to the existing arguments.
    /// </summary>
    /// <param name="options">The command-line tool options to extend.</param>
    /// <param name="arguments">The arguments to append.</param>
    /// <returns>A new <see cref="CommandLineToolOptions"/> instance with the arguments appended.</returns>
    public static CommandLineToolOptions WithArguments(this CommandLineToolOptions options, params string[] arguments)
    {
        return options with
        {
            Arguments = (options.Arguments ?? Array.Empty<string>()).Concat(arguments),
        };
    }
}
