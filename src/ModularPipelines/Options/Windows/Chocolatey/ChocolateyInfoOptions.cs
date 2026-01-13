using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco info' command.
/// Gets package information.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateyInfoOptions : ChocolateyOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChocolateyInfoOptions"/> class
    /// with the specified package name.
    /// </summary>
    /// <param name="package">The name of the package to get information about.</param>
    public ChocolateyInfoOptions(string package)
    {
        Package = package;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "info";

    /// <summary>
    /// Gets the package to get information about.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Package { get; }

    /// <summary>
    /// Gets or sets the source to find the package.
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include pre-release versions.
    /// </summary>
    [CliFlag("--pre")]
    public virtual bool? PreRelease { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to get local package information.
    /// </summary>
    [CliFlag("--local-only")]
    public virtual bool? LocalOnly { get; set; }
}
