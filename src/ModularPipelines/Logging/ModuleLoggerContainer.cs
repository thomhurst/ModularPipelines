using System.Collections.Concurrent;

namespace ModularPipelines.Logging;

/// <summary>
/// Manages the lifecycle of all active module loggers.
/// Maintains a registry of loggers and coordinates their disposal.
/// </summary>
/// <remarks>
/// This container tracks all module loggers created during pipeline execution
/// and ensures they are properly flushed and disposed in the correct order
/// (ordered by last log written time) to maintain logical output ordering.
/// Uses ConcurrentBag for lock-free thread-safe operations during high-concurrency
/// module execution.
/// </remarks>
internal class ModuleLoggerContainer : IModuleLoggerContainer, IDisposable
{
    private readonly ConcurrentBag<ModuleLogger> _loggers = new();
    private int _isDisposed;

    public void FlushAndDisposeAll()
    {
        // Use Interlocked to ensure only one thread disposes
        if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
        {
            return;
        }

        // ToArray() is thread-safe on ConcurrentBag
        var snapshot = _loggers.ToArray();

        foreach (var logger in snapshot.Where(x => x != null).OrderBy(x => x.LastLogWritten))
        {
            logger.Dispose();
        }
    }

    public IModuleLogger? GetLogger(Type type)
    {
        // ToArray() creates a snapshot for safe enumeration
        return _loggers.ToArray().FirstOrDefault(logger => logger.GetType() == type);
    }

    public void AddLogger(ModuleLogger logger)
    {
        // ConcurrentBag.Add is lock-free and thread-safe
        _loggers.Add(logger);
    }

    public void Dispose()
    {
        FlushAndDisposeAll();
    }
}
