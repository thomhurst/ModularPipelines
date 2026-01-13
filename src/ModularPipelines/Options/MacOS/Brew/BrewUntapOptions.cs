using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew untap' command.
/// Removes a tapped formula repository.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewUntapOptions : BrewOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewUntapOptions"/> class
    /// with the specified repository to untap.
    /// </summary>
    /// <param name="repository">The repository to untap (e.g., 'user/repo').</param>
    public BrewUntapOptions(string repository)
    {
        Repository = repository;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "untap";

    /// <summary>
    /// Gets the repository to untap.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Repository { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to force untapping even if formulae are installed from the tap.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
