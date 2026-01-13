using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Base options class for Homebrew commands.
/// Contains common flags available across most brew commands.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("brew")]
public record BrewOptions : CommandLineToolOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to print debugging information.
    /// </summary>
    [CliFlag("--debug")]
    public virtual bool? Debug { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to make some output more quiet.
    /// </summary>
    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to make some output more verbose.
    /// </summary>
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show version information.
    /// </summary>
    [CliFlag("--version")]
    public virtual bool? Version { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show help information.
    /// </summary>
    [CliFlag("--help")]
    public virtual bool? Help { get; set; }
}
