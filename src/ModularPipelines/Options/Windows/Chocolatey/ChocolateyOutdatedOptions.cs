using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco outdated' command.
/// Lists outdated packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateyOutdatedOptions : ChocolateyOptions
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "outdated";

    /// <summary>
    /// Gets or sets the source to check for updates.
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include pre-release versions.
    /// </summary>
    [CliFlag("--pre")]
    public virtual bool? PreRelease { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore pinned packages.
    /// </summary>
    [CliFlag("--ignore-pinned")]
    public virtual bool? IgnorePinned { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore unfound packages.
    /// </summary>
    [CliFlag("--ignore-unfound")]
    public virtual bool? IgnoreUnfound { get; set; }
}
