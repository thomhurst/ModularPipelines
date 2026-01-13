using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco install' command.
/// Installs a package.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateyInstallOptions : ChocolateyOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChocolateyInstallOptions"/> class
    /// with the specified package to install.
    /// </summary>
    /// <param name="package">The name of the package to install.</param>
    public ChocolateyInstallOptions(string package)
    {
        Package = package;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "install";

    /// <summary>
    /// Gets the package to install.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Package { get; }

    /// <summary>
    /// Gets or sets the source to find the package.
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets the version of the package to install.
    /// </summary>
    [CliFlag("--version")]
    public new virtual string? Version { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow pre-release versions.
    /// </summary>
    [CliFlag("--pre")]
    public virtual bool? PreRelease { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to reinstall if already installed.
    /// </summary>
    [CliFlag("--force")]
    public new virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore dependencies.
    /// </summary>
    [CliFlag("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force dependencies.
    /// </summary>
    [CliFlag("--force-dependencies")]
    public virtual bool? ForceDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip PowerShell scripts.
    /// </summary>
    [CliFlag("--skip-automation-scripts")]
    public virtual bool? SkipAutomationScripts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow multiple versions.
    /// </summary>
    [CliFlag("--allow-multiple-versions")]
    public virtual bool? AllowMultipleVersions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow downgrade.
    /// </summary>
    [CliFlag("--allow-downgrade")]
    public virtual bool? AllowDowngrade { get; set; }

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
    /// Gets or sets a value indicating whether to not install dependencies.
    /// </summary>
    [CliFlag("--ignore-checksums")]
    public virtual bool? IgnoreChecksums { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow empty checksums.
    /// </summary>
    [CliFlag("--allow-empty-checksums")]
    public virtual bool? AllowEmptyChecksums { get; set; }

    /// <summary>
    /// Gets or sets the package parameters to pass to the package.
    /// </summary>
    [CliFlag("--package-parameters")]
    public virtual string? PackageParameters { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to apply install arguments to dependencies.
    /// </summary>
    [CliFlag("--apply-install-arguments-to-dependencies")]
    public virtual bool? ApplyInstallArgumentsToDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to apply package parameters to dependencies.
    /// </summary>
    [CliFlag("--apply-package-parameters-to-dependencies")]
    public virtual bool? ApplyPackageParametersToDependencies { get; set; }

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
    /// Gets or sets the install directory.
    /// </summary>
    [CliFlag("--install-directory")]
    public virtual string? InstallDirectory { get; set; }
}
