using ModularPipelines.Options;

namespace ModularPipelines.Extensions;

/// <summary>
/// Provides extension methods for working with command-line options.
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Creates a new <see cref="CommandLineToolOptions"/> with a single argument appended to the existing arguments.
    /// </summary>
    /// <param name="options">The command-line tool options to extend.</param>
    /// <param name="singleArgument">A single argument to append.</param>
    /// <returns>A new <see cref="CommandLineToolOptions"/> instance with the argument appended.</returns>
    public static TOptions WithArguments<TOptions>(this TOptions options, string singleArgument)
        where TOptions : CommandLineToolOptions
    {
        return WithArguments(options, [singleArgument]);
    }

    /// <summary>
    /// Creates a new <see cref="CommandLineToolOptions"/> with additional arguments appended to the existing arguments.
    /// </summary>
    /// <param name="options">The command-line tool options to extend.</param>
    /// <param name="arguments">The arguments to append, or null to append nothing.</param>
    /// <returns>A new <see cref="CommandLineToolOptions"/> instance with the arguments appended.</returns>
    public static TOptions WithArguments<TOptions>(this TOptions options, IEnumerable<string>? arguments)
        where TOptions : CommandLineToolOptions
    {
        return WithArguments(options, arguments?.ToArray() ?? []);
    }

    /// <summary>
    /// Creates a new <see cref="CommandLineToolOptions"/> with additional arguments appended to the existing arguments.
    /// </summary>
    /// <param name="options">The command-line tool options to extend.</param>
    /// <param name="arguments">The arguments to append.</param>
    /// <returns>A new <see cref="CommandLineToolOptions"/> instance with the arguments appended.</returns>
    public static TOptions WithArguments<TOptions>(this TOptions options, params string[] arguments)
        where TOptions : CommandLineToolOptions
    {
        return options with
        {
            Arguments = (options.Arguments ?? Array.Empty<string>()).Concat(arguments).ToArray(),
        };
    }
}
