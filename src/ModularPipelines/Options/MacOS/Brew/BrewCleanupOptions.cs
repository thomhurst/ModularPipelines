using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew cleanup' command.
/// Removes old versions of installed formulae.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewCleanupOptions : BrewOptions
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "cleanup";

    /// <summary>
    /// Gets or sets a value indicating whether to perform a dry run.
    /// </summary>
    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to remove downloads for casks.
    /// </summary>
    [CliFlag("--prune")]
    public virtual int? PruneDays { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to scrub the cache.
    /// </summary>
    [CliFlag("-s")]
    public virtual bool? Scrub { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to remove all cache files.
    /// </summary>
    [CliFlag("--prune-prefix")]
    public virtual bool? PrunePrefix { get; set; }
}
