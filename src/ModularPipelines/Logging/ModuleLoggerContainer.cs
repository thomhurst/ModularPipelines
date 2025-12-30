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
/// Uses ConcurrentDictionary for O(1) thread-safe lookups by type.
/// </remarks>
internal class ModuleLoggerContainer : IModuleLoggerContainer, IDisposable
{
    private readonly ConcurrentDictionary<Type, ModuleLogger> _loggers = new();

    // Interlocked.Exchange provides both atomicity and full memory barrier,
    // making volatile unnecessary for this disposal guard pattern
    private int _isDisposed;

    public void FlushAndDisposeAll()
    {
        // Use Interlocked to ensure only one thread disposes
        if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
        {
            return;
        }

        // Values provides a snapshot for safe enumeration
        foreach (var logger in _loggers.Values.Where(x => x != null).OrderBy(x => x.LastLogWritten))
        {
            logger.Dispose();
        }
    }

    public IModuleLogger? GetLogger(Type type)
    {
        // O(1) lookup via ConcurrentDictionary
        return _loggers.TryGetValue(type, out var logger) ? logger : null;
    }

    public void AddLogger(ModuleLogger logger)
    {
        // Thread-safe add; each module type should only have one logger instance
        _loggers.TryAdd(logger.GetType(), logger);
    }

    public void Dispose()
    {
        FlushAndDisposeAll();
    }
}
