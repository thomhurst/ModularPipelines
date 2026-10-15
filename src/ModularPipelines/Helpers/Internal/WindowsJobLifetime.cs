using Microsoft.Win32.SafeHandles;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Owns command jobs until the pipeline is disposed, including jobs whose root command has exited.
/// </summary>
internal sealed class WindowsJobLifetime : IDisposable
{
    private readonly Lock _lock = new();
    private readonly HashSet<SafeFileHandle> _jobs = [];
    private bool _disposed;

    public void Add(SafeFileHandle job)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                job.Dispose();
                throw new ObjectDisposedException(nameof(WindowsJobLifetime));
            }

            _jobs.Add(job);
        }
    }

    public void Release(SafeFileHandle job)
    {
        lock (_lock)
        {
            _jobs.Remove(job);
            job.Dispose();
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _disposed = true;
            foreach (var job in _jobs)
            {
                job.Dispose();
            }

            _jobs.Clear();
        }
    }
}
