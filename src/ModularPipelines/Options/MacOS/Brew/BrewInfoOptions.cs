using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew info' command.
/// Displays information about a formula or cask.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewInfoOptions : BrewOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewInfoOptions"/> class
    /// with the specified formula or cask.
    /// </summary>
    /// <param name="formulaOrCask">The name of the formula or cask to get information about.</param>
    public BrewInfoOptions(string formulaOrCask)
    {
        FormulaOrCask = formulaOrCask;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "info";

    /// <summary>
    /// Gets the formula or cask to get information about.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string FormulaOrCask { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to show cask information.
    /// </summary>
    [CliFlag("--cask")]
    public virtual bool? Cask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show formula information.
    /// </summary>
    [CliFlag("--formula")]
    public virtual bool? Formula { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to output as JSON.
    /// </summary>
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include analytics data.
    /// </summary>
    [CliFlag("--analytics")]
    public virtual bool? Analytics { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show GitHub info.
    /// </summary>
    [CliFlag("--github")]
    public virtual bool? GitHub { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show all versions.
    /// </summary>
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show installed files.
    /// </summary>
    [CliFlag("--installed")]
    public virtual bool? Installed { get; set; }
}
