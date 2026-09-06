namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Abstraction for executing CLI commands.
/// Allows for mocking in tests and consistent command execution.
/// </summary>
public interface ICliCommandExecutor
{
    /// <summary>
    /// Executes a CLI command and returns the output.
    /// </summary>
    /// <param name="command">The command to execute (e.g., "docker").</param>
    /// <param name="arguments">Arguments for the command (e.g., "run --help").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="workingDirectory">Optional working directory for the command.</param>
    /// <returns>Result containing stdout, stderr, and exit code.</returns>
    Task<CliCommandResult> ExecuteAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default,
        string? workingDirectory = null);

    /// <summary>
    /// Checks if a CLI tool is available on the system.
    /// </summary>
    /// <param name="command">The command to check (e.g., "docker").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the command is available.</returns>
    Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a CLI tool accepts a tool-specific availability probe.
    /// </summary>
    /// <param name="command">The command to check (e.g., "kubectl").</param>
    /// <param name="arguments">Arguments that should complete successfully when the tool is available.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the command is available using the requested probe or the help fallback.</returns>
    Task<bool> IsAvailableAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default) =>
        IsAvailableAsync(command, cancellationToken);
}

/// <summary>
/// Result of executing a CLI command.
/// </summary>
public class CliCommandResult
{
    /// <summary>
    /// Standard output from the command.
    /// </summary>
    public required string StandardOutput { get; init; }

    /// <summary>
    /// Standard error from the command.
    /// </summary>
    public required string StandardError { get; init; }

    /// <summary>
    /// Exit code from the command.
    /// </summary>
    public required int ExitCode { get; init; }

    /// <summary>
    /// Whether the command was abandoned because the executor's timeout elapsed.
    /// </summary>
    public bool TimedOut { get; init; }

    /// <summary>
    /// Whether the command was never attempted because the executor's circuit breaker was open.
    /// </summary>
    public bool CircuitOpen { get; init; }

    /// <summary>
    /// Whether the output is not the command's real response: it timed out or was rejected by
    /// the circuit breaker. Callers treat such output as unavailable rather than as a result.
    /// </summary>
    public bool Unavailable => TimedOut || CircuitOpen;

    /// <summary>
    /// Whether the command executed successfully (exit code 0).
    /// </summary>
    public bool Success => ExitCode == 0;

    /// <summary>
    /// Combined output (stdout + stderr).
    /// </summary>
    public string CombinedOutput => $"{StandardOutput}\n{StandardError}".Trim();
}
