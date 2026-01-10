namespace ModularPipelines.Models;

/// <summary>
/// Represents a fully-built command line ready for execution.
/// This is the output of ICommandBuilder and input to ICommandExecutor.
/// </summary>
public record CommandLine
{
    /// <summary>
    /// The CLI tool to execute (e.g., "git", "docker").
    /// </summary>
    public string Tool { get; }

    /// <summary>
    /// The command arguments.
    /// </summary>
    public IReadOnlyList<string> Arguments { get; }

    /// <summary>
    /// Initializes a new instance with the specified tool and arguments.
    /// Creates a defensive copy to ensure immutability.
    /// </summary>
    /// <param name="tool">The CLI tool to execute.</param>
    /// <param name="arguments">The command arguments.</param>
    public CommandLine(string tool, IEnumerable<string> arguments)
    {
        Tool = tool;
        Arguments = arguments.ToList().AsReadOnly();
    }

    /// <summary>
    /// Returns the command line as a string suitable for display.
    /// </summary>
    public override string ToString()
    {
        return Arguments.Count == 0
            ? Tool
            : $"{Tool} {string.Join(" ", Arguments)}";
    }
}
