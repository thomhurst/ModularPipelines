using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget source' command.
/// Manages package sources.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetSourceOptions : WingetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WingetSourceOptions"/> class
    /// with the specified subcommand.
    /// </summary>
    /// <param name="subCommand">The source subcommand: "list", "add", "remove", "update", "reset", or "export".</param>
    public WingetSourceOptions(string subCommand)
    {
        SubCommand = subCommand;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "source";

    /// <summary>
    /// Gets the source subcommand.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string SubCommand { get; }

    /// <summary>
    /// Gets or sets the source name.
    /// </summary>
    [CliFlag("--name")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the source argument (URL for add).
    /// </summary>
    [CliFlag("--arg")]
    public virtual string? Arg { get; set; }

    /// <summary>
    /// Gets or sets the source type.
    /// </summary>
    [CliFlag("--type")]
    public virtual string? Type { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept source agreements.
    /// </summary>
    [CliFlag("--accept-source-agreements")]
    public virtual bool? AcceptSourceAgreements { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force the operation.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets the header to use for REST source operations.
    /// </summary>
    [CliFlag("--header")]
    public virtual string? Header { get; set; }
}
