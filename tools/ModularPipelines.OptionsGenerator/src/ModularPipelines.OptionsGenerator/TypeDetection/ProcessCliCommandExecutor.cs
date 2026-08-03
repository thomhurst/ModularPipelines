using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Implementation of ICliCommandExecutor that uses System.Diagnostics.Process.
/// </summary>
public class ProcessCliCommandExecutor : ICliCommandExecutor
{
    internal const string ExecutableOverrideVariableName = "MODULARPIPELINES_CLI_EXECUTABLE";
    private static readonly TimeSpan ProcessTreeKillTimeout = TimeSpan.FromSeconds(2);

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
        if (executablePath is null)
        {
            _logger.LogWarning("Command not found: {Command}", command);
            return new CliCommandResult
            {
                StandardOutput = string.Empty,
                StandardError = $"Command not found: {command}",
                ExitCode = -1
            };
        }

        var startInfo = CreateStartInfo(executablePath, arguments, workingDirectory);

        // Disable pagers for CLI tools - many CLIs use pagers by default which hang in non-interactive mode
        startInfo.Environment["AWS_PAGER"] = "";    // AWS CLI
        startInfo.Environment["GIT_PAGER"] = "";    // Git
        startInfo.Environment["NO_COLOR"] = "1";    // Disable color output which can cause parsing issues

        var processLaunch = OperatingSystem.IsWindows()
            ? WindowsJobLauncher.Wrap(startInfo)
            : UnixProcessGroupLauncher.Wrap(startInfo);

        try
        {
            using var process = new Process { StartInfo = processLaunch.StartInfo };
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(_timeout);

            process.Start();
            process.StandardInput.Close();
            var descendantTracker = new DescendantProcessTracker(
                process.Id,
                processLaunch.UsesUnixProcessGroup,
                processLaunch.UsesWindowsJobLauncher);
            var disposeDescendantTracker = true;

            try
            {
                var stdoutTask = process.StandardOutput.ReadToEndAsync(cts.Token);
                var stderrTask = process.StandardError.ReadToEndAsync(cts.Token);

                try
                {
                    await process.WaitForExitAsync(cts.Token);
                    await Task.WhenAll(stdoutTask, stderrTask).WaitAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    disposeDescendantTracker = !await TryKillProcessAsync(
                            process,
                            descendantTracker,
                            command,
                            arguments)
                        .ConfigureAwait(false);
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
            finally
            {
                if (disposeDescendantTracker)
                {
                    descendantTracker.Dispose();
                }
            }
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

    private static string? ResolveExecutablePath(string command)
    {
        var configuredPath = Environment.GetEnvironmentVariable(ExecutableOverrideVariableName);
        if (!string.IsNullOrWhiteSpace(configuredPath)
            && IsOverrideForCommand(command, configuredPath))
        {
            var fullConfiguredPath = Path.GetFullPath(configuredPath);
            return File.Exists(fullConfiguredPath) ? fullConfiguredPath : null;
        }

        return ResolveExecutablePath(
            command,
            Environment.GetEnvironmentVariable("PATH"),
            Environment.GetEnvironmentVariable("PATHEXT"),
            OperatingSystem.IsWindows(),
            Environment.CurrentDirectory);
    }

    internal static bool IsOverrideForCommand(string command, string configuredPath) =>
        Path.GetFileNameWithoutExtension(command)
            .Equals(
                Path.GetFileNameWithoutExtension(configuredPath),
                StringComparison.OrdinalIgnoreCase);

    internal static string? ResolveExecutablePath(
        string command,
        string? searchPath,
        string? pathExtensions,
        bool isWindows,
        string? processDirectory = null)
    {
        if (!isWindows)
        {
            return ResolveFromPath(command, searchPath, processDirectory);
        }

        return WindowsCommandResolver.Resolve(command, processDirectory, searchPath, pathExtensions, isWindows: true);
    }

    internal static ProcessStartInfo CreateStartInfo(
        string executablePath,
        string arguments,
        string? workingDirectory = null,
        bool? isWindows = null,
        string? commandInterpreter = null)
    {
        var useCommandInterpreter = (isWindows ?? OperatingSystem.IsWindows())
            && WindowsCommandResolver.IsCommandScript(executablePath);

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
            RedirectStandardInput = true,
            CreateNoWindow = true
        };

        if (!string.IsNullOrEmpty(workingDirectory))
        {
            startInfo.WorkingDirectory = workingDirectory;
        }

        return startInfo;
    }

    private static string BuildCommandInterpreterArguments(string executablePath, string arguments)
    {
        var command = string.IsNullOrWhiteSpace(arguments)
            ? $"\"{executablePath}\""
            : $"\"{executablePath}\" {arguments}";

        return $"/d /s /c \"{command}\"";
    }

    private static string? ResolveFromPath(
        string command,
        string? searchPath,
        string? processDirectory = null)
    {
        if (Path.IsPathRooted(command))
        {
            return File.Exists(command) ? command : null;
        }

        if (!string.IsNullOrEmpty(Path.GetDirectoryName(command)))
        {
            var candidate = Path.GetFullPath(
                command,
                processDirectory ?? Environment.CurrentDirectory);
            return File.Exists(candidate) ? candidate : null;
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
            {
                return true;
            }

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
    private async Task<bool> TryKillProcessAsync(
        Process process,
        DescendantProcessTracker descendantTracker,
        string command,
        string arguments)
    {
        _logger.LogDebug("Killing timed-out process tree: {Command} {Arguments}", command, arguments);
        var processId = process.Id;
        var processStartTime = TryGetProcessStartTime(process);
        var cleanupOwnsDescendantTracker = false;
        var killTreeTask = Task.Run(() =>
        {
            descendantTracker.KillCapturedDescendants();
            KillProcessTree(processId, processStartTime);
        });
        try
        {
            await killTreeTask.WaitAsync(ProcessTreeKillTimeout).ConfigureAwait(false);
            return false;
        }
        catch (TimeoutException)
        {
            cleanupOwnsDescendantTracker = true;
            _ = killTreeTask.ContinueWith(
                static (task, state) =>
                {
                    _ = task.Exception;
                    ((DescendantProcessTracker) state!).Dispose();
                },
                descendantTracker,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
            _logger.LogWarning(
                "Timed out killing descendants for process: {Command} {Arguments}",
                command,
                arguments);
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogWarning(ex, "Failed to kill process tree: {Command} {Arguments}", command, arguments);
        }

        try
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            // Do not mask the original timeout or cancellation.
            _logger.LogWarning(ex, "Failed to kill process: {Command} {Arguments}", command, arguments);
        }

        return cleanupOwnsDescendantTracker;
    }

    private static DateTime? TryGetProcessStartTime(Process process)
    {
        try
        {
            return process.StartTime;
        }
        catch (Exception exception) when (exception is
            InvalidOperationException
            or System.ComponentModel.Win32Exception)
        {
            return null;
        }
    }

    private static void KillProcessTree(int processId, DateTime? expectedStartTime)
    {
        try
        {
            using var process = Process.GetProcessById(processId);
            if (!process.HasExited
                && MatchesProcessIdentity(expectedStartTime, process.StartTime))
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            // The direct process exited or could not be killed after its descendants were captured.
        }
    }

    internal static bool MatchesProcessIdentity(
        DateTime? expectedStartTime,
        DateTime actualStartTime) =>
        expectedStartTime.HasValue && expectedStartTime.Value == actualStartTime;
}
