using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal sealed class DescendantProcessTracker : IDisposable
{
    private static readonly TimeSpan InitialPollingInterval = TimeSpan.FromMilliseconds(20);
    private static readonly TimeSpan SteadyPollingInterval = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan InitialPollingDuration = TimeSpan.FromMilliseconds(250);
    private readonly Lock _lock = new();
    private readonly TrackedProcess? _rootProcess;
    private readonly DateTime _rootProcessStartTime;
    private readonly Dictionary<ProcessIdentity, TrackedProcess> _capturedProcesses = [];
    private readonly Stopwatch _trackingAge = Stopwatch.StartNew();
    private readonly Timer? _timer;
    private readonly int? _unixProcessGroupId;
    private readonly SafeFileHandle? _windowsJob;
    private bool _disposed;

    public DescendantProcessTracker(
        int rootProcessId,
        bool usesUnixProcessGroup = false,
        bool usesWindowsJobLauncher = false)
    {
        _unixProcessGroupId = usesUnixProcessGroup ? rootProcessId : null;
        if (!ChildProcessSnapshot.IsSupported)
        {
            return;
        }

        Process? rootProcess = null;
        try
        {
            rootProcess = Process.GetProcessById(rootProcessId);
            _rootProcessStartTime = rootProcess.StartTime;
            _rootProcess = new TrackedProcess(
                new ProcessIdentity(rootProcessId, _rootProcessStartTime),
                rootProcess);
            _windowsJob = OperatingSystem.IsWindows() && !usesWindowsJobLauncher
                ? TryCreateWindowsJob(rootProcess)
                : null;
        }
        catch (Exception exception) when (exception is
            ArgumentException
            or InvalidOperationException
            or System.ComponentModel.Win32Exception)
        {
            rootProcess?.Dispose();
            return;
        }

        _timer = new Timer(
            static state => ((DescendantProcessTracker) state!).Poll(),
            this,
            Timeout.InfiniteTimeSpan,
            Timeout.InfiniteTimeSpan);
        CaptureDescendants();
        ScheduleNextPoll();
    }

    public void KillCapturedDescendants()
    {
        CaptureDescendants();
        KillContainedProcesses();
        Process[] capturedProcesses;
        lock (_lock)
        {
            capturedProcesses = [.. _capturedProcesses.Values.Select(static process => process.Process).Reverse()];
        }

        foreach (var process in capturedProcesses)
        {
            TryKill(process);
        }
    }

    public void Dispose()
    {
        Process[] capturedProcesses;
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            capturedProcesses = [.. _capturedProcesses.Values.Select(static process => process.Process)];
            _capturedProcesses.Clear();
        }

        _timer?.Dispose();
        _windowsJob?.Dispose();
        _rootProcess?.Process.Dispose();
        foreach (var process in capturedProcesses)
        {
            process.Dispose();
        }
    }

    private static void TryKill(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (Exception exception) when (exception is
            ArgumentException
            or InvalidOperationException
            or NotSupportedException
            or System.ComponentModel.Win32Exception)
        {
            // The descendant exited while cleanup was in progress.
        }
    }

    private void KillContainedProcesses()
    {
        if (_unixProcessGroupId is { } processGroupId)
        {
            _ = KillProcess(-processGroupId, 9);
        }

        if (_windowsJob is { IsInvalid: false, IsClosed: false })
        {
            _ = TerminateJobObject(_windowsJob, 1);
        }
    }

    private static SafeFileHandle? TryCreateWindowsJob(Process process)
    {
        var job = WindowsJob.TryCreate();
        if (job is null)
        {
            return null;
        }

        try
        {
            if (WindowsJob.TryAssign(job, process.SafeHandle))
            {
                return job;
            }
        }
        catch (InvalidOperationException)
        {
            // The root process exited before it could be assigned.
        }

        job.Dispose();
        return null;
    }

    private void CaptureDescendants()
    {
        try
        {
            var pending = GetCaptureRoots();
            if (pending.Count == 0)
            {
                return;
            }

            var snapshot = ChildProcessSnapshot.Create();
            var visited = new HashSet<ProcessIdentity>();
            while (pending.TryDequeue(out var parentProcess))
            {
                if (!visited.Add(parentProcess.Identity))
                {
                    continue;
                }

                foreach (var childProcessId in snapshot.GetChildProcessIds(parentProcess.Identity.ProcessId))
                {
                    if (TryCaptureProcess(childProcessId, parentProcess, out var childProcess))
                    {
                        pending.Enqueue(childProcess);
                    }
                }
            }
        }
        catch (Exception exception) when (exception is
            IOException
            or UnauthorizedAccessException
            or ArgumentException
            or InvalidOperationException
            or System.ComponentModel.Win32Exception)
        {
            // Process snapshots are inherently racy; the next poll retries.
        }
    }

    private void Poll()
    {
        CaptureDescendants();
        ScheduleNextPoll();
    }

    private void ScheduleNextPoll()
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            var interval = _trackingAge.Elapsed < InitialPollingDuration
                ? InitialPollingInterval
                : SteadyPollingInterval;
            _timer?.Change(interval, Timeout.InfiniteTimeSpan);
        }
    }

    private Queue<TrackedProcess> GetCaptureRoots()
    {
        var pending = new Queue<TrackedProcess>();
        lock (_lock)
        {
            if (_disposed)
            {
                return pending;
            }

            if (_rootProcess is not null)
            {
                EnqueueIfIdentityIsSafe(pending, _rootProcess);
            }

            foreach (var process in _capturedProcesses.Values)
            {
                EnqueueIfIdentityIsSafe(pending, process);
            }
        }

        return pending;
    }

    private static void EnqueueIfIdentityIsSafe(
        Queue<TrackedProcess> pending,
        TrackedProcess process)
    {
        if (TryGetExitState(process.Process, out var hasExited, out var exitTime)
            && CanCaptureChildren(hasExited, exitTime))
        {
            pending.Enqueue(process);
        }
    }

    private bool TryCaptureProcess(
        int processId,
        TrackedProcess parentProcess,
        out TrackedProcess trackedProcess)
    {
        trackedProcess = null!;

        Process? process = null;
        try
        {
            process = Process.GetProcessById(processId);
            var processStartTime = process.StartTime;
            if (!CanBeDescendant(_rootProcessStartTime, processStartTime)
                || !TryGetExitState(
                    parentProcess.Process,
                    out var parentHasExited,
                    out var parentExitTime)
                || !CanCaptureChildren(parentHasExited, parentExitTime)
                || !CanBeChildOfParent(
                    parentProcess.Identity.StartTime,
                    parentExitTime,
                    processStartTime))
            {
                process.Dispose();
                process = null;
                return false;
            }

            var identity = new ProcessIdentity(processId, processStartTime);
            lock (_lock)
            {
                if (_disposed)
                {
                    return false;
                }

                if (_capturedProcesses.TryGetValue(identity, out var existingProcess))
                {
                    trackedProcess = existingProcess;
                    return true;
                }

                trackedProcess = new TrackedProcess(identity, process);
                _capturedProcesses.Add(identity, trackedProcess);
                process = null;
                return true;
            }
        }
        catch (Exception exception) when (exception is
            ArgumentException
            or InvalidOperationException
            or System.ComponentModel.Win32Exception)
        {
            // The process exited while it was being captured.
        }
        finally
        {
            process?.Dispose();
        }

        return false;
    }

    internal static bool CanBeDescendant(
        DateTime rootProcessStartTime,
        DateTime candidateProcessStartTime) =>
        candidateProcessStartTime >= rootProcessStartTime;

    internal static bool CanBeChildOfParent(
        DateTime parentProcessStartTime,
        DateTime? parentProcessExitTime,
        DateTime candidateProcessStartTime) =>
        candidateProcessStartTime >= parentProcessStartTime
        && (!parentProcessExitTime.HasValue
            || candidateProcessStartTime <= parentProcessExitTime.Value);

    internal static bool CanCaptureChildren(bool hasExited, DateTime? exitTime) =>
        !hasExited || exitTime.HasValue;

    private static bool TryGetExitState(
        Process process,
        out bool hasExited,
        out DateTime? exitTime)
    {
        try
        {
            if (!process.HasExited)
            {
                hasExited = false;
                exitTime = null;
                return true;
            }

            hasExited = true;
        }
        catch (Exception exception) when (exception is
            InvalidOperationException
            or System.ComponentModel.Win32Exception)
        {
            hasExited = false;
            exitTime = null;
            return false;
        }

        try
        {
            exitTime = process.ExitTime;
        }
        catch (Exception exception) when (exception is
            InvalidOperationException
            or System.ComponentModel.Win32Exception)
        {
            // Without an exit-time bound, a reused PID cannot be distinguished safely.
            exitTime = null;
        }

        return true;
    }

    private readonly record struct ProcessIdentity(int ProcessId, DateTime StartTime);

    private sealed record TrackedProcess(ProcessIdentity Identity, Process Process);

#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
    [DllImport("libc", EntryPoint = "kill", SetLastError = true)]
    private static extern int KillProcess(int processId, int signal);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool TerminateJobObject(SafeFileHandle job, uint exitCode);
#pragma warning restore SYSLIB1054
}
