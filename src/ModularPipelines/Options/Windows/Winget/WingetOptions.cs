using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Base options class for Windows Package Manager (winget) commands.
/// Contains common flags available across most winget commands.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("winget")]
public record WingetOptions : CommandLineToolOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to disable interactive prompts.
    /// </summary>
    [CliFlag("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; } = true;

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

    /// <summary>
    /// Gets or sets a value indicating whether to show additional logs.
    /// </summary>
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show verbose logs.
    /// </summary>
    [CliFlag("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to wait for user input on exit.
    /// </summary>
    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to open settings.
    /// </summary>
    [CliFlag("--info")]
    public virtual bool? Info { get; set; }
}
