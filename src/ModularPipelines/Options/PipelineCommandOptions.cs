using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Configures global defaults for command execution and logging.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record PipelineCommandOptions
{
    private CommandExecutionOptions? _defaultExecutionOptions;

    /// <summary>
    /// Gets the default logging options for all commands.
    /// Per-call <see cref="CommandExecutionOptions.LogSettings"/> takes precedence.
    /// </summary>
    public CommandLoggingOptions? Logging { get; init; }

    /// <summary>
    /// Gets the default execution options for all commands.
    /// Per-call options take precedence over these global defaults.
    /// </summary>
    public CommandExecutionOptions? Execution
    {
        get => _defaultExecutionOptions;
        init => _defaultExecutionOptions = value is null
            ? null
            : value with
            {
                EnvironmentVariables = value.EnvironmentVariables is null
                    ? null
                    : new ReadOnlyDictionary<string, string?>(
                        new Dictionary<string, string?>(value.EnvironmentVariables)),
            };
    }
}
