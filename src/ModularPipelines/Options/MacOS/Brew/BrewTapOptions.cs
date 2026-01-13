using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew tap' command.
/// Taps a formula repository.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewTapOptions : BrewOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewTapOptions"/> class
    /// with the specified repository to tap.
    /// </summary>
    /// <param name="repository">The repository to tap (e.g., 'user/repo').</param>
    public BrewTapOptions(string repository)
    {
        Repository = repository;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "tap";

    /// <summary>
    /// Gets the repository to tap.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Repository { get; }

    /// <summary>
    /// Gets or sets a custom URL to clone the repository from.
    /// </summary>
    [CliArgument(2, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to perform a full clone.
    /// </summary>
    [CliFlag("--full")]
    public virtual bool? Full { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to perform a shallow clone.
    /// </summary>
    [CliFlag("--shallow")]
    public virtual bool? Shallow { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force tapping even if tap already exists.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force auto-update.
    /// </summary>
    [CliFlag("--force-auto-update")]
    public virtual bool? ForceAutoUpdate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to repair existing tap.
    /// </summary>
    [CliFlag("--repair")]
    public virtual bool? Repair { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to list all taps.
    /// </summary>
    [CliFlag("--list-pinned")]
    public virtual bool? ListPinned { get; set; }
}
