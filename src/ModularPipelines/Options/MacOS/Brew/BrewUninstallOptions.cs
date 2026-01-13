using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew uninstall' command.
/// Uninstalls a formula or cask.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewUninstallOptions : BrewOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewUninstallOptions"/> class
    /// with the specified formula or cask to uninstall.
    /// </summary>
    /// <param name="formulaOrCask">The name of the formula or cask to uninstall.</param>
    public BrewUninstallOptions(string formulaOrCask)
    {
        FormulaOrCask = formulaOrCask;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "uninstall";

    /// <summary>
    /// Gets the formula or cask to uninstall.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string FormulaOrCask { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to uninstall as a cask.
    /// </summary>
    [CliFlag("--cask")]
    public virtual bool? Cask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to uninstall as a formula.
    /// </summary>
    [CliFlag("--formula")]
    public virtual bool? Formula { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force uninstall even if there are dependents.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore dependencies.
    /// </summary>
    [CliFlag("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to delete cask app (not just config).
    /// </summary>
    [CliFlag("--zap")]
    public virtual bool? Zap { get; set; }
}
