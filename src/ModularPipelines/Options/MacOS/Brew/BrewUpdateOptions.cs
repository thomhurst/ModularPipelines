using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew update' command.
/// Fetches the newest version of Homebrew and all formulae.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewUpdateOptions : BrewOptions
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "update";

    /// <summary>
    /// Gets or sets a value indicating whether to force update, even if unnecessary.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to merge changes instead of rebasing.
    /// </summary>
    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to perform a shallow clone for faster update.
    /// </summary>
    [CliFlag("--auto-update")]
    public virtual bool? AutoUpdate { get; set; }
}
