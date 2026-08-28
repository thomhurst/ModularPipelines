namespace ModularPipelines.Options;

/// <summary>
/// Configuration for how a command is executed, independent of tool-specific arguments.
/// </summary>
public record CommandExecutionOptions
{
    internal static TimeSpan DefaultExecutionTimeout { get; } = TimeSpan.FromMinutes(30);

    internal const int DefaultMaxCapturedOutputLength = 1024 * 1024;

    /// <summary>
    /// Gets any EnvironmentVariables to pass to the command.
    /// </summary>
    public IReadOnlyDictionary<string, string?>? EnvironmentVariables { get; init; }

    /// <summary>
    /// Gets the working directory to run the command from.
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <summary>
    /// Gets the credentials used to start the command-line process.
    /// </summary>
    public CommandLineCredentials? CommandLineCredentials { get; init; }

    /// <summary>
    /// Gets logging options for command execution.
    /// </summary>
    public CommandLoggingOptions? Logging { get; init; }

    /// <summary>
    /// Gets if logging input, you can use this to edit how the input is logged.
    /// </summary>
    public Func<string, string>? InputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets if logging output, you can use this to edit how the output is logged.
    /// The complete output stream is retained for this delegate even when
    /// <see cref="MaxCapturedOutputLength"/> bounds the returned command result.
    /// </summary>
    public Func<string, string>? OutputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets a value indicating whether to prefix commands with Sudo.
    /// </summary>
    public bool Sudo { get; init; }

    /// <summary>
    /// Gets a value indicating whether to throw an exception on non-zero exit codes.
    /// </summary>
    public bool ThrowOnNonZeroExitCode { get; init; } = true;

    /// <summary>
    /// Gets the maximum time allowed for the command to complete.
    /// Defaults to 30 minutes.
    /// </summary>
    public TimeSpan? ExecutionTimeout { get; init; } = DefaultExecutionTimeout;

    /// <summary>
    /// Gets the time to wait for graceful shutdown before forcefully terminating.
    /// </summary>
    public TimeSpan GracefulShutdownTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets the maximum number of characters retained from each command output stream.
    /// When output exceeds this limit, the beginning and end are retained with a truncation marker.
    /// Set to 0 or a negative value to retain unlimited output.
    /// </summary>
    public int MaxCapturedOutputLength { get; init; } = DefaultMaxCapturedOutputLength;

    internal bool InternalDryRun { get; set; }

    internal Task? InternalForcefulCancellationReady { get; init; }
}
