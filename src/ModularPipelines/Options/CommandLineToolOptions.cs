namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the command line tool and any arguments it needs.
/// </summary>
public abstract record CommandLineToolOptions(string Tool)
{
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
