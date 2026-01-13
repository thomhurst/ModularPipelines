using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco upgrade' command.
/// Upgrades a package to the latest version.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateyUpgradeOptions : ChocolateyOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChocolateyUpgradeOptions"/> class
    /// with the specified package to upgrade. Use "all" to upgrade all packages.
    /// </summary>
    /// <param name="package">The name of the package to upgrade, or "all" for all packages.</param>
    public ChocolateyUpgradeOptions(string package)
    {
        Package = package;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "upgrade";

    /// <summary>
    /// Gets the package to upgrade (or "all" for all packages).
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Package { get; }

    /// <summary>
    /// Gets or sets the source to find the package.
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets the version to upgrade to.
    /// </summary>
    [CliFlag("--version")]
    public new virtual string? Version { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow pre-release versions.
    /// </summary>
    [CliFlag("--pre")]
    public virtual bool? PreRelease { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore dependencies.
    /// </summary>
    [CliFlag("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip PowerShell scripts.
    /// </summary>
    [CliFlag("--skip-automation-scripts")]
    public virtual bool? SkipAutomationScripts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to fail on unfound.
    /// </summary>
    [CliFlag("--fail-on-unfound")]
    public virtual bool? FailOnUnfound { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to fail on not installed.
    /// </summary>
    [CliFlag("--fail-on-not-installed")]
    public virtual bool? FailOnNotInstalled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore checksums.
    /// </summary>
    [CliFlag("--ignore-checksums")]
    public virtual bool? IgnoreChecksums { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow empty checksums.
    /// </summary>
    [CliFlag("--allow-empty-checksums")]
    public virtual bool? AllowEmptyChecksums { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore pinned packages.
    /// </summary>
    [CliFlag("--ignore-pinned")]
    public virtual bool? IgnorePinned { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to exclude pre-release.
    /// </summary>
    [CliFlag("--except")]
    public virtual string? Except { get; set; }

    /// <summary>
    /// Gets or sets the install arguments to pass to the native installer.
    /// </summary>
    [CliFlag("--install-arguments")]
    public virtual string? InstallArguments { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to override arguments.
    /// </summary>
    [CliFlag("--override-arguments")]
    public virtual bool? OverrideArguments { get; set; }

    /// <summary>
    /// Gets or sets the package parameters to pass to the package.
    /// </summary>
    [CliFlag("--package-parameters")]
    public virtual string? PackageParameters { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip virus check.
    /// </summary>
    [CliFlag("--skip-virus-check")]
    public virtual bool? SkipVirusCheck { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use 32bit (x86) package.
    /// </summary>
    [CliFlag("--x86")]
    public virtual bool? X86 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to exclude self from upgrade.
    /// </summary>
    [CliFlag("--exclude-chocolatey-packages-during-upgrade-all")]
    public virtual bool? ExcludeChocolateyPackagesDuringUpgradeAll { get; set; }
}
