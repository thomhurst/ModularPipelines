using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Common options for the Windows Command Prompt executable.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("cmd")]
public record CmdOptions : CommandLineToolOptions
{
    /// <summary>
    /// Gets a value indicating whether command echoing is disabled.
    /// </summary>
    [CliFlag("/q")]
    public virtual bool DisableEcho { get; init; }

    /// <summary>
    /// Gets a value indicating whether Command Prompt exits after running the script.
    /// </summary>
    [CliFlag("/c")]
    public virtual bool StopAfter { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether output uses Unicode.
    /// </summary>
    [CliFlag("/u")]
    public virtual bool Unicode { get; init; }

    /// <summary>
    /// Gets a value indicating whether output uses ANSI.
    /// </summary>
    [CliFlag("/a")]
    public virtual bool Ansi { get; init; }

    /// <summary>
    /// Gets a value indicating whether AutoRun commands are disabled.
    /// </summary>
    [CliFlag("/d")]
    public virtual bool DisableAutoRunCommands { get; init; }
}
