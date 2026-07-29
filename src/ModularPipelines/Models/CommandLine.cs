namespace ModularPipelines.Models;

/// <summary>
/// Represents a fully-built command line ready for execution.
/// This is the output of the internal command-line builder and input to ICommandExecutor.
/// </summary>
public record CommandLine
{
    /// <summary>
    /// Gets the CLI tool to execute (e.g., "git", "docker").
    /// </summary>
    public string Tool { get; }

    /// <summary>
    /// Gets the command arguments.
    /// </summary>
    public IReadOnlyList<string> Arguments { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="CommandLine"/> class.
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
    /// <returns>The command line as a display string.</returns>
    public override string ToString()
    {
        return Arguments.Count == 0
            ? Tool
            : $"{Tool} {string.Join(" ", Arguments)}";
    }
}
