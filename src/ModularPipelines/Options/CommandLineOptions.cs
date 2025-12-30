using CliWrap;
using ModularPipelines.Enums;

namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the context of a command.
/// </summary>
public record CommandLineOptions
{
    /// <summary>
    /// Gets any EnvironmentVariables to pass to the command.
    /// </summary>
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }

    /// <summary>
    /// Gets the working directory to run the command from.
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <inheritdoc cref="CommandLineCredentials"/>>
    public Credentials? CommandLineCredentials { get; init; }

    /// <summary>
    /// Gets or sets logging options for command execution.
    /// </summary>
    /// <remarks>
    /// <para>Use <see cref="CommandLoggingOptions.Silent"/> to suppress all logging,
    /// or <see cref="CommandLoggingOptions.Diagnostic"/> for maximum verbosity.</para>
    /// <para>If not set, falls back to <see cref="PipelineOptions.DefaultLoggingOptions"/>.</para>
    /// </remarks>
    public CommandLoggingOptions? LogSettings { get; init; }

    /// <summary>
    /// Gets controls command logging
    /// e.g. Logging = CommandLogging.Input | CommandLogging.Output | CommandLogging.Error.
    /// </summary>
    [Obsolete("Use LogSettings instead. This property will be removed in a future version.")]
    public CommandLogging? CommandLogging { get; init; }

    /// <summary>
    /// Gets if logging input, you can use this to edit how the input is logged
    /// E.g. if you want to replace a secret value with stars.
    /// </summary>
    public Func<string, string>? InputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets if logging output, you can use this to edit how the output is logged
    /// E.g. if you want to replace a secret value with stars.
    /// </summary>
    public Func<string, string>? OutputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether prefix commands with Sudo to run with elevated privileges for Unix systems.
    /// </summary>
    public bool Sudo { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception on non-zero exit codes.
    /// </summary>
    public bool ThrowOnNonZeroExitCode { get; init; } = true;

    internal bool InternalDryRun { get; set; }
}