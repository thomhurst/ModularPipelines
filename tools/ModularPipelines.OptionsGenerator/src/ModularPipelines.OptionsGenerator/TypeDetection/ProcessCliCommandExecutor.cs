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
        CancellationToken cancellationToken = default,
        string? workingDirectory = null)
    {
        _logger.LogDebug("Executing: {Command} {Arguments} (WorkingDir: {WorkingDir})", command, arguments, workingDirectory ?? "default");

        var executablePath = ResolveExecutablePath(command);
        var startInfo = CreateStartInfo(executablePath, arguments, workingDirectory);

        // Disable pagers for CLI tools - many CLIs use pagers by default which hang in non-interactive mode
        startInfo.Environment["AWS_PAGER"] = "";    // AWS CLI
        startInfo.Environment["GIT_PAGER"] = "";    // Git
        startInfo.Environment["NO_COLOR"] = "1";    // Disable color output which can cause parsing issues

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
            if (process.ExitCode != 0)
            {
                _logger.LogWarning(
                    "Command failed: {Command} {Arguments} (exit code {ExitCode}). stderr: {StandardError}",
                    command,
                    arguments,
                    process.ExitCode,
                    stderr);
            }

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
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
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

    private static string ResolveExecutablePath(string command) => ResolveExecutablePath(
        command,
        Environment.GetEnvironmentVariable("PATH"),
        Environment.GetEnvironmentVariable("PATHEXT"),
        OperatingSystem.IsWindows());

    internal static string ResolveExecutablePath(
        string command,
        string? searchPath,
        string? pathExtensions,
        bool isWindows)
    {
        if (!isWindows || Path.IsPathRooted(command) || Path.HasExtension(command))
        {
            return ResolveFromPath(command, searchPath) ?? command;
        }

        var extensions = pathExtensions?
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            ?? [".COM", ".EXE", ".BAT", ".CMD"];

        foreach (var pathDirectory in GetPathDirectories(searchPath))
        {
            foreach (var extension in extensions)
            {
                var candidate = Path.Combine(pathDirectory, command + extension.ToLowerInvariant());
                if (File.Exists(candidate))
                {
                    return Path.GetFullPath(candidate);
                }
            }
        }

        return command;
    }

    internal static ProcessStartInfo CreateStartInfo(
        string executablePath,
        string arguments,
        string? workingDirectory = null,
        bool? isWindows = null,
        string? commandInterpreter = null)
    {
        var extension = Path.GetExtension(executablePath);
        var useCommandInterpreter = (isWindows ?? OperatingSystem.IsWindows())
            && IsCommandScript(extension);

        var startInfo = new ProcessStartInfo
        {
            FileName = useCommandInterpreter
                ? commandInterpreter ?? Environment.GetEnvironmentVariable("COMSPEC") ?? "cmd.exe"
                : executablePath,
            Arguments = useCommandInterpreter
                ? BuildCommandInterpreterArguments(executablePath, arguments)
                : arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        if (!string.IsNullOrEmpty(workingDirectory))
        {
            startInfo.WorkingDirectory = workingDirectory;
        }

        return startInfo;
    }

    private static bool IsCommandScript(string extension) =>
        extension.Equals(".bat", StringComparison.OrdinalIgnoreCase)
        || extension.Equals(".cmd", StringComparison.OrdinalIgnoreCase);

    private static string BuildCommandInterpreterArguments(string executablePath, string arguments)
    {
        var command = string.IsNullOrWhiteSpace(arguments)
            ? $"\"{executablePath}\""
            : $"\"{executablePath}\" {arguments}";

        return $"/d /s /c \"{command}\"";
    }

    private static string? ResolveFromPath(string command, string? searchPath)
    {
        if (Path.IsPathRooted(command))
        {
            return File.Exists(command) ? command : null;
        }

        foreach (var pathDirectory in GetPathDirectories(searchPath))
        {
            var candidate = Path.Combine(pathDirectory, command);
            if (File.Exists(candidate))
            {
                return Path.GetFullPath(candidate);
            }
        }

        return null;
    }

    private static IEnumerable<string> GetPathDirectories(string? searchPath) => searchPath?
        .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(pathDirectory => pathDirectory.Trim('"'))
        ?? [];
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
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            // Log but don't throw - we don't want to mask the original timeout exception
            _logger.LogWarning(ex, "Failed to kill process: {Command} {Arguments}", command, arguments);
        }
    }
}
