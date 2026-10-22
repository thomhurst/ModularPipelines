using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for executing PowerShell commands using the pwsh executable.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("pwsh")]
[CliGlobalOptions]
public record PowerShellOptions : CommandLineToolOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether PowerShell skips loading profiles.
    /// Defaults to true. Set to false to load the user's PowerShell profiles.
    /// </summary>
    [CliFlag("-NoProfile")]
    public virtual bool NoProfile { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether PowerShell disables interactive prompts.
    /// Defaults to true. Set to false to allow interactive input.
    /// </summary>
    [CliFlag("-NonInteractive")]
    public virtual bool NonInteractive { get; init; } = true;
}
