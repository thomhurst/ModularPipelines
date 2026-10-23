namespace ModularPipelines;

/// <summary>
/// Represents a fully-built command line ready for execution.
/// This is the output of the internal command-line builder and input to the command execution pipeline.
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

    /// <inheritdoc />
    public virtual bool Equals(CommandLine? other)
    {
        return other is not null
            && EqualityContract == other.EqualityContract
            && StringComparer.Ordinal.Equals(Tool, other.Tool)
            && Arguments.SequenceEqual(other.Arguments, StringComparer.Ordinal);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = default(HashCode);
        hash.Add(EqualityContract);
        hash.Add(Tool, StringComparer.Ordinal);

        foreach (var argument in Arguments)
        {
            hash.Add(argument, StringComparer.Ordinal);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Returns the tool name with arguments redacted for diagnostic display.
    /// </summary>
    /// <remarks>
    /// Raw arguments can contain unregistered secrets. Use <see cref="Arguments"/> for execution
    /// and <see cref="CommandInvocation.CommandInput"/> for a secret-masked command display.
    /// </remarks>
    /// <returns>The tool name followed by a redaction marker when arguments are present.</returns>
    public override string ToString()
    {
        return Arguments.Count == 0
            ? Tool
            : $"{Tool} <redacted>";
    }
}
