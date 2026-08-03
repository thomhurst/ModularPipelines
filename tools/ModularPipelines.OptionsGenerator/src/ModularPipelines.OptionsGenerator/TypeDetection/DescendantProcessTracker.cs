using System.Diagnostics;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal sealed class DescendantProcessTracker : IDisposable
{
    private static readonly TimeSpan InitialPollingInterval = TimeSpan.FromMilliseconds(20);
    private static readonly TimeSpan SteadyPollingInterval = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan InitialPollingDuration = TimeSpan.FromMilliseconds(250);
    private readonly Lock _lock = new();
    private readonly int _rootProcessId;
    private readonly Dictionary<int, Process> _capturedProcesses = [];
    private readonly Stopwatch _trackingAge = Stopwatch.StartNew();
    private readonly Timer? _timer;
    private bool _disposed;

    public DescendantProcessTracker(int rootProcessId)
    {
        _rootProcessId = rootProcessId;
        if (ChildProcessSnapshot.IsSupported)
        {
            _timer = new Timer(
                static state => ((DescendantProcessTracker) state!).Poll(),
                this,
                Timeout.InfiniteTimeSpan,
                Timeout.InfiniteTimeSpan);
            CaptureDescendants();
            ScheduleNextPoll();
        }
    }

    public void KillCapturedDescendants()
    {
        CaptureDescendants();
        Process[] capturedProcesses;
        lock (_lock)
        {
            capturedProcesses = [.. _capturedProcesses.Values.Reverse()];
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
            capturedProcesses = [.. _capturedProcesses.Values];
            _capturedProcesses.Clear();
        }

        _timer?.Dispose();
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
            var visited = new HashSet<int>();
            while (pending.TryDequeue(out var parentProcessId))
            {
                if (!visited.Add(parentProcessId))
                {
                    continue;
                }

                foreach (var childProcessId in snapshot.GetChildProcessIds(parentProcessId))
                {
                    pending.Enqueue(childProcessId);
                    CaptureProcess(childProcessId);
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

    private Queue<int> GetCaptureRoots()
    {
        var pending = new Queue<int>();
        lock (_lock)
        {
            if (_disposed)
            {
                return pending;
            }

            pending.Enqueue(_rootProcessId);
            foreach (var processId in _capturedProcesses.Keys)
            {
                pending.Enqueue(processId);
            }
        }

        return pending;
    }

    private void CaptureProcess(int processId)
    {
        lock (_lock)
        {
            if (_disposed || _capturedProcesses.ContainsKey(processId))
            {
                return;
            }
        }

        Process? process = null;
        try
        {
            process = Process.GetProcessById(processId);
            if (process.HasExited)
            {
                process.Dispose();
                return;
            }

            lock (_lock)
            {
                if (!_disposed && _capturedProcesses.TryAdd(processId, process))
                {
                    process = null;
                }
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
    }
}
