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
    /// <returns>Result containing stdout, stderr, and exit code.</returns>
    Task<CliCommandResult> ExecuteAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a CLI tool is available on the system.
    /// </summary>
    /// <param name="command">The command to check (e.g., "docker").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the command is available.</returns>
    Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default);
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
    /// Whether the command executed successfully (exit code 0).
    /// </summary>
    public bool Success => ExitCode == 0;

    /// <summary>
    /// Combined output (stdout + stderr).
    /// </summary>
    public string CombinedOutput => $"{StandardOutput}\n{StandardError}".Trim();
}
