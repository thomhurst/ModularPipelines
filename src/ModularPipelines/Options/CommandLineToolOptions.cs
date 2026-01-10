namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the command line tool and any arguments it needs.
/// </summary>
public abstract record CommandLineToolOptions
{
    /// <summary>
    /// Gets the CLI tool name. May be null if specified via [CliTool] attribute instead.
    /// </summary>
    public string? Tool { get; }

    /// <summary>
    /// Gets the command parts (subcommands) for the tool.
    /// </summary>
    public string[]? CommandParts { get; init; }

    /// <summary>
    /// Gets used for providing switches and arguments to the tool.
    /// </summary>
    public IEnumerable<string>? Arguments { get; init; }

    /// <summary>
    /// Gets used for command line tools that support -- syntax.
    /// </summary>
    public IEnumerable<string>? RunSettings { get; init; }

    /// <summary>
    /// Creates options with tool name specified via [CliTool] attribute.
    /// </summary>
    protected CommandLineToolOptions()
    {
        Tool = null;
    }

    /// <summary>
    /// Creates options with explicit tool name (legacy, for backward compatibility).
    /// </summary>
    /// <param name="tool">The name or path of the command-line tool to execute.</param>
    protected CommandLineToolOptions(string tool)
    {
        Tool = tool;
    }
}
