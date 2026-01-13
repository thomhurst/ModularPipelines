using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget uninstall' command.
/// Uninstalls a package.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetUninstallOptions : WingetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WingetUninstallOptions"/> class.
    /// At least one of Id, Name, or Moniker must be specified.
    /// </summary>
    public WingetUninstallOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WingetUninstallOptions"/> class
    /// with the specified package ID.
    /// </summary>
    /// <param name="id">The package ID to uninstall.</param>
    public WingetUninstallOptions(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "uninstall";

    /// <summary>
    /// Gets or sets the package ID to uninstall.
    /// </summary>
    [CliFlag("--id")]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets or sets the package name to uninstall.
    /// </summary>
    [CliFlag("--name")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the package moniker to uninstall.
    /// </summary>
    [CliFlag("--moniker")]
    public virtual string? Moniker { get; set; }

    /// <summary>
    /// Gets or sets the version to uninstall.
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
    /// Gets or sets a value indicating whether to request interactive mode.
    /// </summary>
    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to request silent mode.
    /// </summary>
    [CliFlag("--silent")]
    public virtual bool? Silent { get; set; }

    /// <summary>
    /// Gets or sets the log file location.
    /// </summary>
    [CliFlag("--log")]
    public virtual string? Log { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force uninstall.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to purge all files and data.
    /// </summary>
    [CliFlag("--purge")]
    public virtual bool? Purge { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to preserve data files.
    /// </summary>
    [CliFlag("--preserve")]
    public virtual bool? Preserve { get; set; }

    /// <summary>
    /// Gets or sets the product code to uninstall.
    /// </summary>
    [CliFlag("--product-code")]
    public virtual string? ProductCode { get; set; }

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
