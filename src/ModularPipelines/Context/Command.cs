using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using CliWrap;
using CliWrap.Exceptions;
using Microsoft.Win32.SafeHandles;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

/// <summary>
/// Orchestrates command-line tool execution by coordinating argument building,
/// placeholder replacement, and command execution.
/// </summary>
internal sealed class Command : ICommand, ICommandContext
{
    private readonly ICommandLogger _commandLogger;
    private readonly ICommandLineBuilder _commandLineBuilder;
    private readonly ISecretProvider _secretProvider;
    private readonly ISecretRegistry _secretRegistry;

    public Command(
        ICommandLogger commandLogger,
        ICommandLineBuilder commandLineBuilder,
        ISecretProvider secretProvider,
        ISecretRegistry secretRegistry)
    {
        _commandLogger = commandLogger;
        _commandLineBuilder = commandLineBuilder;
        _secretProvider = secretProvider;
        _secretRegistry = secretRegistry;
    }

    public async Task<CommandResult> ExecuteCommandLineTool(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        var execOpts = executionOptions ?? new CommandExecutionOptions();

        _secretRegistry.AddSecrets(_secretProvider.GetSecretsInObject(options));

        var commandLine = _commandLineBuilder.Build(options);
        var resolvedTool = commandLine.Tool;
        var parsedArgs = commandLine.Arguments.ToList();

        string tool;
        if (execOpts.Sudo)
        {
            tool = "sudo";
            parsedArgs.Insert(0, resolvedTool);
        }
        else
        {
            tool = resolvedTool;
        }

        var command = CliCommandFactory.Create(tool, parsedArgs, execOpts);

        if (execOpts.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(execOpts.WorkingDirectory);
        }

        if (execOpts.CommandLineCredentials != null)
        {
            command = command.WithCredentials(execOpts.CommandLineCredentials.ToCliWrapCredentials());
        }

        if (execOpts.InternalDryRun)
        {
            _commandLogger.Log(
                options: options,
                execOpts: execOpts,
                inputToLog: execOpts.InputLoggingManipulator == null ? command.ToString() : execOpts.InputLoggingManipulator(command.ToString()),
                exitCode: 0,
                runTime: TimeSpan.Zero,
                standardOutput: "Dummy Output Response",
                standardError: "Dummy Error Response",
                commandWorkingDirPath: command.WorkingDirPath
            );

            return new CommandResult(command);
        }

        return await Of(command, options, execOpts, cancellationToken).ConfigureAwait(false);
    }

    private async Task<CommandResult> Of(
        CliWrap.Command command,
        CommandLineToolOptions options,
        CommandExecutionOptions execOpts,
        CancellationToken cancellationToken = default)
    {
        StringBuilder standardOutputStringBuilder = new();
        StringBuilder standardErrorStringBuilder = new();
        var stopwatch = Stopwatch.StartNew();

        var standardOutput = string.Empty;
        var standardError = string.Empty;

        var inputToLog = execOpts.InputLoggingManipulator == null ? command.ToString() : execOpts.InputLoggingManipulator(command.ToString());

        // Only create timeout token if ExecutionTimeout is specified to avoid unnecessary allocations
        using var timeoutCancellationToken = execOpts.ExecutionTimeout.HasValue
            ? new CancellationTokenSource(execOpts.ExecutionTimeout.Value)
            : null;

        // Link the timeout token with the passed cancellation token, or just wrap the original if no timeout
        using var linkedCancellationToken = timeoutCancellationToken != null
            ? CancellationTokenSource.CreateLinkedTokenSource(timeoutCancellationToken.Token, cancellationToken)
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        using var forcefulCancellationToken = new CancellationTokenSource();
        using var processTreeTerminator = new ProcessTreeTerminator();
        using var processTreeCancellationRegistration =
            forcefulCancellationToken.Token.Register(processTreeTerminator.Kill);

        var registration = linkedCancellationToken.Token.Register(() =>
        {
            try
            {
                if (forcefulCancellationToken.Token.CanBeCanceled)
                {
                    forcefulCancellationToken.CancelAfter(execOpts.GracefulShutdownTimeout);
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignored
            }
        });
        await using (registration.ConfigureAwait(false))
        {
            try
            {
                var executionTask = command
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(standardOutputStringBuilder))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(standardErrorStringBuilder))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(
                        configureStartInfo: startInfo =>
                        {
                            if (OperatingSystem.IsWindows())
                            {
                                startInfo.CreateNewProcessGroup = true;
                            }
                        },
                        configureProcess: processTreeTerminator.Attach,
                        forcefulCancellationToken: CancellationToken.None,
                        gracefulCancellationToken: linkedCancellationToken.Token);
                using var descendantCaptureRegistration =
                    linkedCancellationToken.Token.Register(processTreeTerminator.CaptureDescendants);
                var result = await executionTask.ConfigureAwait(false);

                await WaitForForcefulCancellationAsync(
                    linkedCancellationToken.Token,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = standardOutputStringBuilder.ToString();
                standardError = standardErrorStringBuilder.ToString();

                _commandLogger.Log(
                    options: options,
                    execOpts: execOpts,
                    inputToLog: inputToLog,
                    exitCode: result.ExitCode,
                    runTime: result.RunTime,
                    standardOutput: standardOutput,
                    standardError: standardError,
                    commandWorkingDirPath: command.WorkingDirPath
                );

                if (result.ExitCode != 0 && execOpts.ThrowOnNonZeroExitCode)
                {
                    var input = inputToLog;
                    throw new CommandException(input, result.ExitCode, result.RunTime, standardOutput, standardError);
                }

                return new CommandResult(command, result, standardOutput, standardError);
            }
            catch (CommandExecutionException e)
            {
                await WaitForForcefulCancellationAsync(
                    linkedCancellationToken.Token,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = standardOutputStringBuilder.ToString();
                standardError = standardErrorStringBuilder.ToString();

                _commandLogger.Log(
                    options: options,
                    execOpts: execOpts,
                    inputToLog: inputToLog,
                    exitCode: e.ExitCode,
                    runTime: stopwatch.Elapsed,
                    standardOutput: standardOutput,
                    standardError: standardError,
                    commandWorkingDirPath: command.WorkingDirPath
                );

                throw new CommandException(inputToLog, e.ExitCode, stopwatch.Elapsed, standardOutput, standardError, e);
            }
            catch (Exception e) when (e is not CommandExecutionException and not CommandException)
            {
                await WaitForForcefulCancellationAsync(
                    linkedCancellationToken.Token,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = standardOutputStringBuilder.ToString();
                standardError = standardErrorStringBuilder.ToString();

                _commandLogger.Log(
                    options: options,
                    execOpts: execOpts,
                    inputToLog: inputToLog,
                    exitCode: -1,
                    runTime: stopwatch.Elapsed,
                    standardOutput: standardOutput,
                    standardError: standardError,
                    commandWorkingDirPath: command.WorkingDirPath
                );

                throw new CommandException(inputToLog, -1, stopwatch.Elapsed, standardOutput, standardError, e);
            }
        }
    }

    private static async Task WaitForForcefulCancellationAsync(
        CancellationToken gracefulCancellationToken,
        CancellationToken forcefulCancellationToken)
    {
        if (!gracefulCancellationToken.IsCancellationRequested ||
            forcefulCancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, forcefulCancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (forcefulCancellationToken.IsCancellationRequested)
        {
            // The graceful shutdown window elapsed.
        }
    }

    private sealed class ProcessTreeTerminator : IDisposable
    {
        private readonly Lock _lock = new();
        private readonly Dictionary<int, Process> _descendants = [];
        private Process? _process;
        private SafeFileHandle? _windowsJob;
        private bool _killRequested;

        public void Attach(Process process)
        {
            var windowsJob = OperatingSystem.IsWindows() ? TryCreateWindowsJob(process) : null;

            lock (_lock)
            {
                _process = process;
                _windowsJob = windowsJob;

                if (!_killRequested)
                {
                    return;
                }
            }

            TryKill(process, windowsJob);
        }

        public void CaptureDescendants()
        {
            if (OperatingSystem.IsWindows())
            {
                return;
            }

            Process? process;

            lock (_lock)
            {
                process = _process;
            }

            if (process is null)
            {
                return;
            }

            try
            {
                foreach (var processId in GetDescendantProcessIds(process.Id))
                {
                    Process? descendant = null;

                    try
                    {
                        descendant = Process.GetProcessById(processId);
                        var capturedDescendant = descendant;
                        var killDescendant = false;

                        lock (_lock)
                        {
                            if (_descendants.TryAdd(processId, descendant))
                            {
                                killDescendant = _killRequested;
                            }
                            else
                            {
                                descendant.Dispose();
                            }

                            descendant = null;
                        }

                        if (killDescendant)
                        {
                            TryKill(capturedDescendant, null);
                        }
                    }
                    catch (ArgumentException)
                    {
                        // The descendant exited while it was being captured.
                    }
                    finally
                    {
                        descendant?.Dispose();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // The root process exited before its descendants could be captured.
            }
        }

        public void Kill()
        {
            Process? process;
            SafeFileHandle? windowsJob;
            Process[] descendants;

            lock (_lock)
            {
                _killRequested = true;
                process = _process;
                windowsJob = _windowsJob;
                descendants = [.. _descendants.Values];
            }

            if (process is not null)
            {
                TryKill(process, windowsJob);
            }

            foreach (var descendant in descendants)
            {
                TryKill(descendant, null);
            }
        }

        public void Dispose()
        {
            SafeFileHandle? windowsJob;
            Process[] descendants;

            lock (_lock)
            {
                windowsJob = _windowsJob;
                descendants = [.. _descendants.Values];
                _windowsJob = null;
                _process = null;
                _descendants.Clear();
            }

            windowsJob?.Dispose();

            foreach (var descendant in descendants)
            {
                descendant.Dispose();
            }
        }

        private static HashSet<int> GetDescendantProcessIds(int rootProcessId)
        {
            var pendingProcessIds = new Queue<int>();
            var descendantProcessIds = new HashSet<int>();
            pendingProcessIds.Enqueue(rootProcessId);

            while (pendingProcessIds.TryDequeue(out var parentProcessId))
            {
                foreach (var childProcessId in GetChildProcessIds(parentProcessId))
                {
                    if (descendantProcessIds.Add(childProcessId))
                    {
                        pendingProcessIds.Enqueue(childProcessId);
                    }
                }
            }

            return descendantProcessIds;
        }

        private static int[] GetChildProcessIds(int parentProcessId)
        {
            if (OperatingSystem.IsLinux())
            {
                var childrenFile = $"/proc/{parentProcessId}/task/{parentProcessId}/children";

                try
                {
                    var processIds = File.ReadAllText(childrenFile)
                        .Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries);
                    return Array.ConvertAll(processIds, int.Parse);
                }
                catch (IOException)
                {
                    return [];
                }
                catch (UnauthorizedAccessException)
                {
                    return [];
                }
            }

            if (OperatingSystem.IsMacOS())
            {
                return MacNativeMethods.GetChildProcessIds(parentProcessId);
            }

            return [];
        }

        [SupportedOSPlatform("windows")]
        private static SafeFileHandle? TryCreateWindowsJob(Process process)
        {
            var job = WindowsNativeMethods.CreateJobObject(nint.Zero, null);
            if (job.IsInvalid)
            {
                job.Dispose();
                return null;
            }

            try
            {
                if (WindowsNativeMethods.AssignProcessToJobObject(job, process.SafeHandle))
                {
                    return job;
                }
            }
            catch (InvalidOperationException)
            {
                // The process exited before it could be assigned.
            }

            job.Dispose();
            return null;
        }

        private static void TryKill(Process process, SafeFileHandle? windowsJob)
        {
            try
            {
                if (OperatingSystem.IsWindows() &&
                    windowsJob is { IsInvalid: false, IsClosed: false } &&
                    WindowsNativeMethods.TerminateJobObject(windowsJob, 1))
                {
                    return;
                }

                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException)
            {
                // The process already exited.
            }
            catch (Win32Exception)
            {
                // The process already exited or could not be terminated.
            }
            catch (NotSupportedException)
            {
                // The process is remote.
            }
        }

        private static class WindowsNativeMethods
        {
#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern SafeFileHandle CreateJobObject(nint jobAttributes, string? name);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AssignProcessToJobObject(
                SafeFileHandle job,
                SafeProcessHandle process);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool TerminateJobObject(SafeFileHandle job, uint exitCode);
#pragma warning restore SYSLIB1054
        }

        [SupportedOSPlatform("macos")]
        private static class MacNativeMethods
        {
            public static int[] GetChildProcessIds(int parentProcessId)
            {
                var processCount = ProcListChildPids(parentProcessId, null, 0);
                if (processCount <= 0)
                {
                    return [];
                }

                var processIds = new int[processCount];
                var actualProcessCount =
                    ProcListChildPids(parentProcessId, processIds, processIds.Length * sizeof(int));
                return actualProcessCount > 0
                    ? processIds[..Math.Min(actualProcessCount, processIds.Length)]
                    : [];
            }

#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
            [DllImport("libproc", EntryPoint = "proc_listchildpids")]
            private static extern int ProcListChildPids(
                int parentProcessId,
                int[]? processIds,
                int byteCount);
#pragma warning restore SYSLIB1054
        }
    }
}
