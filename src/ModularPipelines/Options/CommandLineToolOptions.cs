namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the command line tool and any arguments it needs.
/// Tool name can be specified via [CliTool] attribute on the options class (preferred)
/// or via the Tool property for runtime-configured tools.
/// </summary>
public abstract record CommandLineToolOptions
{
    /// <summary>
    /// Gets the CLI tool name for runtime-configured tools.
    /// Prefer using [CliTool] attribute on the options class instead.
    /// </summary>
    public string? Tool { get; init; }

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
}
