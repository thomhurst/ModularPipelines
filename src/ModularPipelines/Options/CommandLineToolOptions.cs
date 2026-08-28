namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the command line tool and any arguments it needs.
/// Static command identities use [CliTool] on the tool base and [CliSubCommand] on
/// command options. Runtime Tool and CommandParts values override those attributes.
/// </summary>
public record CommandLineToolOptions
{
    /// <summary>
    /// Initializes options for a strongly typed command whose tool is supplied by metadata.
    /// </summary>
    protected CommandLineToolOptions()
    {
    }

    /// <summary>
    /// Initializes options for an arbitrary command-line tool.
    /// </summary>
    /// <param name="tool">The name or path of the command-line tool to execute.</param>
    public CommandLineToolOptions(string tool)
    {
        Tool = tool;
    }

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
    /// unless recognized tool options are hoisted when <see cref="ArgumentsContainToolOptions"/>
    /// is enabled. These tokens precede <see cref="RunSettings"/> and terminal options.
    /// </summary>
    public IEnumerable<string>? Arguments { get; init; }

    /// <summary>
    /// Gets manual tokens whose placement is controlled by their command-line phase.
    /// Use this for unmodeled options on strongly typed or generated option records.
    /// </summary>
    public IEnumerable<AdditionalCommandLineArgument>? AdditionalArguments { get; init; }

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
