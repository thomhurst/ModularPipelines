using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew list' command.
/// Lists installed formulae and casks.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewListOptions : BrewOptions
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "list";

    /// <summary>
    /// Gets or sets a value indicating whether to list casks.
    /// </summary>
    [CliFlag("--cask")]
    public virtual bool? Cask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to list formulae.
    /// </summary>
    [CliFlag("--formula")]
    public virtual bool? Formula { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show full paths.
    /// </summary>
    [CliFlag("--full-name")]
    public virtual bool? FullName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show versions.
    /// </summary>
    [CliFlag("--versions")]
    public virtual bool? Versions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to list multiple versions.
    /// </summary>
    [CliFlag("--multiple")]
    public virtual bool? Multiple { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to only show pinned formulae.
    /// </summary>
    [CliFlag("--pinned")]
    public virtual bool? Pinned { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to output as one entry per line.
    /// </summary>
    [CliFlag("-1")]
    public virtual bool? OnePerLine { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to output as long format.
    /// </summary>
    [CliFlag("-l")]
    public virtual bool? Long { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to reverse the order.
    /// </summary>
    [CliFlag("-r")]
    public virtual bool? Reverse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to sort by time modified.
    /// </summary>
    [CliFlag("-t")]
    public virtual bool? SortByTime { get; set; }
}
