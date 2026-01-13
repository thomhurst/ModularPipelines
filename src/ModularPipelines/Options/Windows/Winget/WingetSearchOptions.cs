using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget search' command.
/// Searches for packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetSearchOptions : WingetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WingetSearchOptions"/> class.
    /// </summary>
    public WingetSearchOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WingetSearchOptions"/> class
    /// with the specified query.
    /// </summary>
    /// <param name="query">The search query.</param>
    public WingetSearchOptions(string query)
    {
        Query = query;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "search";

    /// <summary>
    /// Gets or sets the search query.
    /// </summary>
    [CliFlag("--query")]
    public virtual string? Query { get; set; }

    /// <summary>
    /// Gets or sets the package ID to search.
    /// </summary>
    [CliFlag("--id")]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets or sets the package name to search.
    /// </summary>
    [CliFlag("--name")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the package moniker to search.
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
