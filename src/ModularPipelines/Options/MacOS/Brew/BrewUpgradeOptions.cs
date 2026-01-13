using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew upgrade' command.
/// Upgrades outdated formulae and casks.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewUpgradeOptions : BrewOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewUpgradeOptions"/> class.
    /// </summary>
    public BrewUpgradeOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrewUpgradeOptions"/> class
    /// with a specific formula or cask to upgrade.
    /// </summary>
    /// <param name="formulaOrCask">The name of the formula or cask to upgrade.</param>
    public BrewUpgradeOptions(string formulaOrCask)
    {
        FormulaOrCask = formulaOrCask;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "upgrade";

    /// <summary>
    /// Gets or sets the formula or cask to upgrade. If not specified, all outdated packages are upgraded.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? FormulaOrCask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to upgrade casks.
    /// </summary>
    [CliFlag("--cask")]
    public virtual bool? Cask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to upgrade formulae.
    /// </summary>
    [CliFlag("--formula")]
    public virtual bool? Formula { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force upgrade even if already latest.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to perform a dry run.
    /// </summary>
    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to upgrade casks with auto-updates.
    /// </summary>
    [CliFlag("--greedy")]
    public virtual bool? Greedy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to fetch the latest version.
    /// </summary>
    [CliFlag("--fetch-HEAD")]
    public virtual bool? FetchHead { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore pinned formulae.
    /// </summary>
    [CliFlag("--ignore-pinned")]
    public virtual bool? IgnorePinned { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to build from source.
    /// </summary>
    [CliFlag("--build-from-source")]
    public virtual bool? BuildFromSource { get; set; }
}
