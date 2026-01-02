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

    /// <summary>
    /// Gets or sets the maximum time allowed for the command to complete.
    /// </summary>
    /// <remarks>
    /// <para>When set, the command will be cancelled if it exceeds this duration.</para>
    /// <para>If the command does not complete within the timeout, a <see cref="System.OperationCanceledException"/> will be thrown.</para>
    /// <para>If not set (null), the command will run until completion or until the passed cancellation token is cancelled.</para>
    /// </remarks>
    public TimeSpan? ExecutionTimeout { get; init; }

    /// <summary>
    /// Gets or sets the time to wait for graceful shutdown before forcefully terminating the process.
    /// </summary>
    /// <remarks>
    /// <para>When a command is cancelled (either via <see cref="ExecutionTimeout"/> or an external cancellation token),
    /// the process is first asked to terminate gracefully. If it does not terminate within this duration,
    /// it will be forcefully killed.</para>
    /// <para>Default is 30 seconds.</para>
    /// </remarks>
    public TimeSpan GracefulShutdownTimeout { get; init; } = TimeSpan.FromSeconds(30);

    internal bool InternalDryRun { get; set; }
}