using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget install' command.
/// Installs a package.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetInstallOptions : WingetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WingetInstallOptions"/> class.
    /// At least one of Id, Name, or Moniker must be specified.
    /// </summary>
    public WingetInstallOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WingetInstallOptions"/> class
    /// with the specified package ID.
    /// </summary>
    /// <param name="id">The package ID to install.</param>
    public WingetInstallOptions(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "install";

    /// <summary>
    /// Gets or sets the package ID to install.
    /// </summary>
    [CliFlag("--id")]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets or sets the package name to install.
    /// </summary>
    [CliFlag("--name")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the package moniker to install.
    /// </summary>
    [CliFlag("--moniker")]
    public virtual string? Moniker { get; set; }

    /// <summary>
    /// Gets or sets the version to install.
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
    /// Gets or sets the locale to install.
    /// </summary>
    [CliFlag("--locale")]
    public virtual string? Locale { get; set; }

    /// <summary>
    /// Gets or sets the log file location.
    /// </summary>
    [CliFlag("--log")]
    public virtual string? Log { get; set; }

    /// <summary>
    /// Gets or sets custom switches to pass to the installer.
    /// </summary>
    [CliFlag("--custom")]
    public virtual string? Custom { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to override installer arguments.
    /// </summary>
    [CliFlag("--override")]
    public virtual string? Override { get; set; }

    /// <summary>
    /// Gets or sets the installation location.
    /// </summary>
    [CliFlag("--location")]
    public virtual string? Location { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force install even if already installed.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept package agreements.
    /// </summary>
    [CliFlag("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept source agreements.
    /// </summary>
    [CliFlag("--accept-source-agreements")]
    public virtual bool? AcceptSourceAgreements { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip dependencies.
    /// </summary>
    [CliFlag("--ignore-security-hash")]
    public virtual bool? IgnoreSecurityHash { get; set; }

    /// <summary>
    /// Gets or sets the scope for installation (user or machine).
    /// </summary>
    [CliFlag("--scope")]
    public virtual string? Scope { get; set; }

    /// <summary>
    /// Gets or sets the architecture to install.
    /// </summary>
    [CliFlag("--architecture")]
    public virtual string? Architecture { get; set; }

    /// <summary>
    /// Gets or sets the installer type to use.
    /// </summary>
    [CliFlag("--installer-type")]
    public virtual string? InstallerType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip hash validation.
    /// </summary>
    [CliFlag("--allow-reboot")]
    public virtual bool? AllowReboot { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to download only.
    /// </summary>
    [CliFlag("--download-only")]
    public virtual bool? DownloadOnly { get; set; }

    /// <summary>
    /// Gets or sets the header to use for REST source operations.
    /// </summary>
    [CliFlag("--header")]
    public virtual string? Header { get; set; }
}
