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
    /// Gets manual tokens appended after generated non-terminal options and operands,
    /// and before <see cref="RunSettings"/> and terminal options.
    /// </summary>
    public IEnumerable<string>? Arguments { get; init; }

    /// <summary>
    /// Gets manual tokens whose placement is controlled by their command-line phase.
    /// Use this for unmodeled options on strongly typed or generated option records.
    /// </summary>
    public IEnumerable<AdditionalCommandLineArgument>? AdditionalArguments { get; init; }

    /// <summary>
    /// Gets used for command line tools that support -- syntax.
    /// </summary>
    public IEnumerable<string>? RunSettings { get; init; }
}
