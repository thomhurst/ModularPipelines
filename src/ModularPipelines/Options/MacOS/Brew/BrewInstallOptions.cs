using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew install' command.
/// Installs a formula or cask.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewInstallOptions : BrewOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewInstallOptions"/> class
    /// with the specified formula or cask to install.
    /// </summary>
    /// <param name="formulaOrCask">The name of the formula or cask to install.</param>
    public BrewInstallOptions(string formulaOrCask)
    {
        FormulaOrCask = formulaOrCask;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "install";

    /// <summary>
    /// Gets the formula or cask to install.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string FormulaOrCask { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to install as a cask (GUI application).
    /// </summary>
    [CliFlag("--cask")]
    public virtual bool? Cask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to install as a formula (command-line tool).
    /// </summary>
    [CliFlag("--formula")]
    public virtual bool? Formula { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip installing any cask dependencies.
    /// </summary>
    [CliFlag("--skip-cask-deps")]
    public virtual bool? SkipCaskDeps { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to install without quarantine attribute.
    /// </summary>
    [CliFlag("--no-quarantine")]
    public virtual bool? NoQuarantine { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to download and install from HEAD commit.
    /// </summary>
    [CliFlag("--HEAD")]
    public virtual bool? Head { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force install even if already installed.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip build dependencies.
    /// </summary>
    [CliFlag("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to only download and cache the file.
    /// </summary>
    [CliFlag("--fetch-HEAD")]
    public virtual bool? FetchHead { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to build from source rather than bottle.
    /// </summary>
    [CliFlag("--build-from-source")]
    public virtual bool? BuildFromSource { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to prefer bottle over building from source.
    /// </summary>
    [CliFlag("--force-bottle")]
    public virtual bool? ForceBottle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include test dependencies.
    /// </summary>
    [CliFlag("--include-test")]
    public virtual bool? IncludeTest { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to produce a dry run with no actions.
    /// </summary>
    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to create a symlink to the application.
    /// </summary>
    [CliFlag("--appdir")]
    public virtual string? AppDir { get; set; }
}
