using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows;

/// <summary>
/// Base class for Windows installer options providing common UI suppression flags.
/// </summary>
/// <remarks>
/// Windows installers support various flags to suppress the user interface during silent installations.
/// This base class provides these common flags that are shared between MSI and EXE installers.
/// </remarks>
[ExcludeFromCodeCoverage]
public abstract record WindowsInstallerOptionsBase : CommandLineToolOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to disable the user interface during installation.
    /// When enabled, applies multiple silent/quiet flags for compatibility with various installer types.
    /// Default: true.
    /// </summary>
    [CliFlag("/exenoui")]
    public virtual bool? DisableUserInterface { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to prevent automatic restart after installation.
    /// Default: true.
    /// </summary>
    [CliFlag("/norestart")]
    public virtual bool NoRestart { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to restart applications that were shut down during installation.
    /// Default: true.
    /// </summary>
    [CliFlag("/restartapplications")]
    public virtual bool RestartApplications { get; init; } = true;

    [CliFlag("/qn")]
    internal virtual bool? DisableUserInterface2 => DisableUserInterface;

    [CliFlag("/quiet")]
    internal virtual bool? DisableUserInterface3 => DisableUserInterface;

    [CliFlag("/silent")]
    internal virtual bool? DisableUserInterface4 => DisableUserInterface;

    [CliFlag("/verysilent")]
    internal virtual bool? DisableUserInterface5 => DisableUserInterface;

    [CliFlag("/suppressmsgboxes")]
    internal virtual bool? DisableUserInterface6 => DisableUserInterface;

    [CliFlag("/passive")]
    internal virtual bool? DisableUserInterface7 => DisableUserInterface;

    [CliFlag("/sp-")]
    internal virtual bool? DisableUserInterface8 => DisableUserInterface;
}
