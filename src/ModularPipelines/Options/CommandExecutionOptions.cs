using System.Text;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration for how a command is executed, independent of tool-specific arguments.
/// </summary>
/// <remarks>Standard input, passwords, and environment variables are redacted when printing this record.</remarks>
public record CommandExecutionOptions
{
    private IReadOnlyCollection<int> _successExitCodes = [0];

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
    /// Gets the encoding used to decode standard output and standard error. Defaults to UTF-8.
    /// A byte order mark in either stream takes precedence over this encoding.
    /// </summary>
    public Encoding OutputEncoding { get; init; } = Encoding.UTF8;

    /// <summary>
    /// Gets text to write to standard input as UTF-8 without a byte order mark or an added newline.
    /// Standard input closes after the text is written. Null closes it immediately.
    /// The text is never included in the command line. To mask echoed input in logs and failure diagnostics,
    /// set <see cref="StandardInputIsSecret"/> or register individual sensitive values with
    /// <see cref="ModularPipelines.Secrets.ISecretRegistry"/> before execution.
    /// </summary>
    public string? StandardInput { get; init; }

    /// <summary>
    /// Gets a value indicating whether to register all of <see cref="StandardInput"/> for secret masking.
    /// Defaults to false so scripts and documents do not expand into pipeline-wide secret patterns.
    /// Set to true for passwords or other small secret payloads. Registration retains the input and its
    /// encoded and per-line variants for the pipeline lifetime; for large input, register only sensitive
    /// values with <see cref="ModularPipelines.Secrets.ISecretRegistry"/> instead.
    /// </summary>
    public bool StandardInputIsSecret { get; init; }

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
    /// Gets a value indicating whether to run commands through sudo in non-interactive mode.
    /// </summary>
    /// <remarks>
    /// Uses <c>sudo -n</c>, which fails instead of prompting when authentication is required.
    /// Requests preservation of the non-null variables explicitly supplied in <see cref="EnvironmentVariables"/>
    /// with <c>--preserve-env=&lt;keys&gt;</c>. Null values remove variables from sudo's input environment.
    /// Variable names containing commas are not supported. Values are not included in the command arguments.
    /// Environment changes from command interceptors refresh preservation while the generated sudo prefix is retained.
    /// Sudo's security policy still controls environment preservation and may override variables such as PATH.
    /// Requires a sudo implementation that supports these options and an existing authentication timestamp
    /// or a policy that permits execution without a password.
    /// </remarks>
    public bool Sudo { get; init; }

    /// <summary>
    /// Gets a value indicating whether to throw an exception for exit codes outside
    /// <see cref="SuccessExitCodes"/>. Set to false to disable exit-code exceptions.
    /// </summary>
    public bool ThrowOnNonZeroExitCode { get; init; } = true;

    /// <summary>
    /// Gets the accepted exit codes. Defaults to only zero. A supplied collection replaces this default;
    /// include zero explicitly to continue accepting it. An empty collection accepts no exit codes.
    /// These codes also determine success in command logs and telemetry, even when
    /// <see cref="ThrowOnNonZeroExitCode"/> is false.
    /// </summary>
    public IReadOnlyCollection<int> SuccessExitCodes
    {
        get => _successExitCodes;
        init => _successExitCodes = value ?? throw new ArgumentNullException(nameof(SuccessExitCodes));
    }

    /// <summary>
    /// Gets the maximum time allowed for the command to complete.
    /// Defaults to 30 minutes. Set to <see langword="null"/> or <see cref="Timeout.InfiniteTimeSpan"/> to disable the timeout.
    /// Finite values must be positive and no greater than 4,294,967,294 milliseconds.
    /// </summary>
    /// <remarks>
    /// When the timeout elapses, the command is stopped and a <see cref="TimeoutException"/> naming this
    /// option is thrown. Raise it for long-running builds, test runs, or publishes.
    /// </remarks>
    public TimeSpan? ExecutionTimeout { get; init; } = DefaultExecutionTimeout;

    /// <summary>
    /// Gets the time to wait for graceful shutdown before forcefully terminating.
    /// Must be non-negative and no greater than 4,294,967,294 milliseconds, or
    /// <see cref="Timeout.InfiniteTimeSpan"/> to wait indefinitely. Zero schedules immediate forceful termination.
    /// Ignored on Windows, where cancellation immediately schedules forceful termination because
    /// commands cannot receive a graceful interrupt.
    /// </summary>
    public TimeSpan GracefulShutdownTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets the maximum number of characters retained from each command output stream.
    /// When output exceeds this limit, the beginning and end are retained with a truncation marker.
    /// Set to 0 or a negative value to retain unlimited output.
    /// </summary>
    public int MaxCapturedOutputLength { get; init; } = DefaultMaxCapturedOutputLength;

    /// <summary>
    /// Gets the time allowed for standard output and error to reach EOF after the direct child exits.
    /// Defaults to two seconds. When the deadline expires, remaining reads stop and the result's
    /// <see cref="CommandResult.OutputMayBeIncomplete"/> is set. The drain deadline itself does not terminate descendants.
    /// Set to <see cref="Timeout.InfiniteTimeSpan"/> to wait for EOF without a drain deadline.
    /// </summary>
    /// <remarks>
    /// After the direct child exits normally, background descendants in the command's Windows job object
    /// can continue running until pipeline disposal, which terminates them. On Linux and macOS, pipeline
    /// disposal does not terminate descendants left running after the direct child exits.
    /// Explicitly stop background servers or daemons during cleanup for consistent behavior across platforms.
    /// </remarks>
    public TimeSpan OutputDrainTimeout { get; init; } = TimeSpan.FromSeconds(2);

    internal bool InternalDryRun { get; set; }

    internal Task? InternalForcefulCancellationReady { get; init; }

    internal bool ShouldThrowForExitCode(int exitCode) =>
        ThrowOnNonZeroExitCode && !SuccessExitCodes.Contains(exitCode);

    /// <summary>
    /// Appends diagnostic members while redacting sensitive execution values.
    /// </summary>
    /// <param name="builder">The builder receiving the record members.</param>
    /// <returns>True because members were appended.</returns>
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append("EnvironmentVariables = ").Append(EnvironmentVariables is null ? "<none>" : "<redacted>");
        builder.Append(", WorkingDirectory = ").Append(WorkingDirectory);
        builder.Append(", OutputEncoding = ").Append(OutputEncoding);
        builder.Append(", StandardInput = ").Append(StandardInput is null ? "<none>" : "<redacted>");
        builder.Append(", StandardInputIsSecret = ").Append(StandardInputIsSecret);
        builder.Append(", CommandLineCredentials = ").Append(CommandLineCredentials);
        builder.Append(", Logging = ").Append(Logging);
        builder.Append(", InputLoggingManipulator = ").Append(InputLoggingManipulator);
        builder.Append(", OutputLoggingManipulator = ").Append(OutputLoggingManipulator);
        builder.Append(", Sudo = ").Append(Sudo);
        builder.Append(", ThrowOnNonZeroExitCode = ").Append(ThrowOnNonZeroExitCode);
        builder.Append(", SuccessExitCodes = ").Append(SuccessExitCodes);
        builder.Append(", ExecutionTimeout = ").Append(ExecutionTimeout);
        builder.Append(", GracefulShutdownTimeout = ").Append(GracefulShutdownTimeout);
        builder.Append(", MaxCapturedOutputLength = ").Append(MaxCapturedOutputLength);
        builder.Append(", OutputDrainTimeout = ").Append(OutputDrainTimeout);
        return true;
    }
}
