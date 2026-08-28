using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Configures global defaults for command execution and logging.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record PipelineCommandOptions
{
    /// <summary>
    /// Gets the default logging options for all commands.
    /// Per-call <see cref="CommandExecutionOptions.Logging"/> takes precedence.
    /// </summary>
    public CommandLoggingOptions? Logging { get; init; }
}
