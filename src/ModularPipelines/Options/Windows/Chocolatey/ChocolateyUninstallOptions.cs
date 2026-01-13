using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco uninstall' command.
/// Uninstalls a package.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateyUninstallOptions : ChocolateyOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChocolateyUninstallOptions"/> class
    /// with the specified package to uninstall.
    /// </summary>
    /// <param name="package">The name of the package to uninstall.</param>
    public ChocolateyUninstallOptions(string package)
    {
        Package = package;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "uninstall";

    /// <summary>
    /// Gets the package to uninstall.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Package { get; }

    /// <summary>
    /// Gets or sets the version of the package to uninstall.
    /// </summary>
    [CliFlag("--version")]
    public new virtual string? Version { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to uninstall all versions.
    /// </summary>
    [CliFlag("--all-versions")]
    public virtual bool? AllVersions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to uninstall dependencies.
    /// </summary>
    [CliFlag("--remove-dependencies")]
    public virtual bool? RemoveDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip PowerShell scripts.
    /// </summary>
    [CliFlag("--skip-automation-scripts")]
    public virtual bool? SkipAutomationScripts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force removal even if it affects other packages.
    /// </summary>
    [CliFlag("--force-dependencies")]
    public virtual bool? ForceDependencies { get; set; }

    /// <summary>
    /// Gets or sets the uninstall arguments to pass to the native uninstaller.
    /// </summary>
    [CliFlag("--uninstall-arguments")]
    public virtual string? UninstallArguments { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to override uninstall arguments.
    /// </summary>
    [CliFlag("--override-arguments")]
    public virtual bool? OverrideArguments { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use autouninstaller.
    /// </summary>
    [CliFlag("--use-autouninstaller")]
    public virtual bool? UseAutoUninstaller { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip autouninstaller.
    /// </summary>
    [CliFlag("--skip-autouninstaller")]
    public virtual bool? SkipAutoUninstaller { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to fail on autouninstaller failure.
    /// </summary>
    [CliFlag("--fail-on-autouninstaller")]
    public virtual bool? FailOnAutoUninstaller { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore autouninstaller failure.
    /// </summary>
    [CliFlag("--ignore-autouninstaller-failure")]
    public virtual bool? IgnoreAutoUninstallerFailure { get; set; }
}
