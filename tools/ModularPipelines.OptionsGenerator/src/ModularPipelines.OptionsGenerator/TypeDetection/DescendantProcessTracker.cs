using System.Diagnostics;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal sealed class DescendantProcessTracker : IDisposable
{
    private static readonly TimeSpan PollingInterval = TimeSpan.FromMilliseconds(5);
    private readonly Lock _lock = new();
    private readonly int _rootProcessId;
    private readonly Dictionary<int, Process> _capturedProcesses = [];
    private readonly Timer? _timer;
    private bool _disposed;

    public DescendantProcessTracker(int rootProcessId)
    {
        _rootProcessId = rootProcessId;
        if (OperatingSystem.IsLinux())
        {
            CaptureDescendants();
            _timer = new Timer(
                static state => ((DescendantProcessTracker) state!).CaptureDescendants(),
                this,
                TimeSpan.Zero,
                PollingInterval);
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
        if (!OperatingSystem.IsLinux())
        {
            return;
        }

        try
        {
            var childrenByParent = GetLinuxProcesses()
                .ToLookup(static process => process.ParentProcessId);
            var pending = GetCaptureRoots();
            var visited = new HashSet<int>();
            while (pending.TryDequeue(out var parentProcessId))
            {
                if (!visited.Add(parentProcessId))
                {
                    continue;
                }

                foreach (var child in childrenByParent[parentProcessId])
                {
                    pending.Enqueue(child.ProcessId);
                    CaptureProcess(child.ProcessId);
                }
            }
        }
        catch (Exception exception) when (exception is
            IOException or UnauthorizedAccessException or ArgumentException)
        {
            // Process snapshots are inherently racy; the next poll retries.
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

    private static IEnumerable<(int ProcessId, int ParentProcessId)> GetLinuxProcesses()
    {
        foreach (var processDirectory in Directory.EnumerateDirectories("/proc"))
        {
            if (!int.TryParse(Path.GetFileName(processDirectory), out var processId)
                || !TryGetParentProcessId(processDirectory, out var parentProcessId))
            {
                continue;
            }

            yield return (processId, parentProcessId);
        }
    }

    private static bool TryGetParentProcessId(
        string processDirectory,
        out int parentProcessId)
    {
        parentProcessId = default;
        string stat;
        try
        {
            stat = File.ReadAllText(Path.Combine(processDirectory, "stat"));
        }
        catch (Exception exception) when (exception is
            IOException or UnauthorizedAccessException)
        {
            return false;
        }

        var commandEnd = stat.LastIndexOf(')');
        if (commandEnd < 0)
        {
            return false;
        }

        var fields = stat[(commandEnd + 1)..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return fields.Length > 1 && int.TryParse(fields[1], out parentProcessId);
    }
}
