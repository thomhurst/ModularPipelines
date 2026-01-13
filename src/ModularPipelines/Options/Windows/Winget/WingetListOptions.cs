using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget list' command.
/// Lists installed packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetListOptions : WingetOptions
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "list";

    /// <summary>
    /// Gets or sets the package ID to filter.
    /// </summary>
    [CliFlag("--id")]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets or sets the package name to filter.
    /// </summary>
    [CliFlag("--name")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the package moniker to filter.
    /// </summary>
    [CliFlag("--moniker")]
    public virtual string? Moniker { get; set; }

    /// <summary>
    /// Gets or sets the source to use.
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use exact match.
    /// </summary>
    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    /// <summary>
    /// Gets or sets the query to filter packages.
    /// </summary>
    [CliFlag("--query")]
    public virtual string? Query { get; set; }

    /// <summary>
    /// Gets or sets the tag to filter packages.
    /// </summary>
    [CliFlag("--tag")]
    public virtual string? Tag { get; set; }

    /// <summary>
    /// Gets or sets the command to filter packages.
    /// </summary>
    [CliFlag("--command")]
    public virtual string? Command { get; set; }

    /// <summary>
    /// Gets or sets the count of results to show.
    /// </summary>
    [CliFlag("--count")]
    public virtual int? Count { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to check for available upgrades.
    /// </summary>
    [CliFlag("--upgrade-available")]
    public virtual bool? UpgradeAvailable { get; set; }

    /// <summary>
    /// Gets or sets the header to use for REST source operations.
    /// </summary>
    [CliFlag("--header")]
    public virtual string? Header { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept source agreements.
    /// </summary>
    [CliFlag("--accept-source-agreements")]
    public virtual bool? AcceptSourceAgreements { get; set; }
}
