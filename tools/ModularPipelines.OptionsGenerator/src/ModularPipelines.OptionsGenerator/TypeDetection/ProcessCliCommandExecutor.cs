using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Implementation of ICliCommandExecutor that uses System.Diagnostics.Process.
/// </summary>
public class ProcessCliCommandExecutor : ICliCommandExecutor
{
    private readonly ILogger<ProcessCliCommandExecutor> _logger;
    private readonly TimeSpan _timeout;

    public ProcessCliCommandExecutor(ILogger<ProcessCliCommandExecutor> logger, TimeSpan? timeout = null)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _timeout = timeout ?? TimeSpan.FromSeconds(30);
    }

    public async Task<CliCommandResult> ExecuteAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing: {Command} {Arguments}", command, arguments);

        var startInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        try
        {
            using var process = new Process { StartInfo = startInfo };
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(_timeout);

            process.Start();

            var stdoutTask = process.StandardOutput.ReadToEndAsync(cts.Token);
            var stderrTask = process.StandardError.ReadToEndAsync(cts.Token);

            try
            {
                await process.WaitForExitAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Kill the process to prevent resource leak
                TryKillProcess(process, command, arguments);
                throw;
            }

            var stdout = await stdoutTask;
            var stderr = await stderrTask;

            _logger.LogDebug("Command completed with exit code {ExitCode}", process.ExitCode);

            return new CliCommandResult
            {
                StandardOutput = stdout,
                StandardError = stderr,
                ExitCode = process.ExitCode
            };
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Command timed out or cancelled: {Command} {Arguments}", command, arguments);
            return new CliCommandResult
            {
                StandardOutput = string.Empty,
                StandardError = "Command timed out or cancelled",
                ExitCode = -1
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute command: {Command} {Arguments}", command, arguments);
            return new CliCommandResult
            {
                StandardOutput = string.Empty,
                StandardError = ex.Message,
                ExitCode = -1
            };
        }
    }

    public async Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to get version/help to check if command exists
            var result = await ExecuteAsync(command, "--version", cancellationToken);
            if (result.Success)
                return true;

            // Some commands don't support --version, try --help
            result = await ExecuteAsync(command, "--help", cancellationToken);
            return result.ExitCode != -1; // -1 indicates execution failure (command not found)
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to kill a process gracefully, falling back to force kill.
    /// Logs but swallows exceptions to prevent masking the original error.
    /// </summary>
    private void TryKillProcess(Process process, string command, string arguments)
    {
        try
        {
            if (!process.HasExited)
            {
                _logger.LogDebug("Killing timed-out process: {Command} {Arguments}", command, arguments);
                process.Kill(entireProcessTree: true);
            }
        }
        catch (Exception ex)
        {
            // Log but don't throw - we don't want to mask the original timeout exception
            _logger.LogWarning(ex, "Failed to kill process: {Command} {Arguments}", command, arguments);
        }
    }
}
