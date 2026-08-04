namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the command line tool and any arguments it needs.
/// Static command identities use [CliTool] on the tool base and [CliSubCommand] on
/// command options. Runtime Tool and CommandParts values override those attributes.
/// </summary>
public abstract record CommandLineToolOptions
{
    /// <summary>
    /// Gets the CLI tool name for runtime-configured tools. A non-null value overrides
    /// the nearest [CliTool] attribute in the options type hierarchy.
    /// </summary>
    public string? Tool { get; init; }

    /// <summary>
    /// Gets the command parts (subcommands) for the tool. A non-null value overrides
    /// a preferred [CliCommandAlias] and [CliSubCommand], in that order.
    /// </summary>
    public IReadOnlyList<string>? CommandParts { get; init; }

    /// <summary>
    /// Gets used for providing switches and arguments to the tool.
    /// </summary>
    public IEnumerable<string>? Arguments { get; init; }

    /// <summary>
    /// Gets whether option-shaped tokens in <see cref="Arguments"/> are options for this tool.
    /// When enabled, recognized options can be moved before an end-of-options marker emitted by
    /// a structured argument. Leave disabled when the arguments are passed through to another tool.
    /// </summary>
    public bool ArgumentsContainToolOptions { get; init; }

    /// <summary>
    /// Gets whether <see cref="Arguments"/> intentionally contains an end-of-options
    /// marker. This distinguishes a marker from a <c>--</c> token used as an option value.
    /// </summary>
    public bool ArgumentsContainOptionTerminator { get; init; }

    /// <summary>
    /// Gets pass-through values rendered after one <c>--</c> option terminator.
    /// Null or empty values emit no terminator.
    /// </summary>
    public IEnumerable<string>? RunSettings { get; init; }
}
