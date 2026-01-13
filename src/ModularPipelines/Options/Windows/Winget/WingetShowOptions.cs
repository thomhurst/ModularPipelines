using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget show' command.
/// Shows information about a package.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetShowOptions : WingetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WingetShowOptions"/> class.
    /// At least one of Id, Name, or Moniker must be specified.
    /// </summary>
    public WingetShowOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WingetShowOptions"/> class
    /// with the specified package ID.
    /// </summary>
    /// <param name="id">The package ID to show.</param>
    public WingetShowOptions(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "show";

    /// <summary>
    /// Gets or sets the package ID to show.
    /// </summary>
    [CliFlag("--id")]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets or sets the package name to show.
    /// </summary>
    [CliFlag("--name")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the package moniker to show.
    /// </summary>
    [CliFlag("--moniker")]
    public virtual string? Moniker { get; set; }

    /// <summary>
    /// Gets or sets the version to show.
    /// </summary>
    [CliFlag("--version")]
    public new virtual string? Version { get; set; }

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
    /// Gets or sets a value indicating whether to show all versions.
    /// </summary>
    [CliFlag("--versions")]
    public virtual bool? Versions { get; set; }

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
