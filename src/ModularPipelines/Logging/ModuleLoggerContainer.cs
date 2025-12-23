namespace ModularPipelines.Logging;

/// <summary>
/// Manages the lifecycle of all active module loggers.
/// Maintains a registry of loggers and coordinates their disposal.
/// </summary>
/// <remarks>
/// This container tracks all module loggers created during pipeline execution
/// and ensures they are properly flushed and disposed in the correct order
/// (ordered by last log written time) to maintain logical output ordering.
/// </remarks>
internal class ModuleLoggerContainer : IModuleLoggerContainer, IDisposable
{
    private readonly List<ModuleLogger> _loggers = new();
    private bool _isDisposed;

    public void FlushAndDisposeAll()
    {
        lock (_loggers)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }

        List<ModuleLogger> snapshot;
        lock (_loggers)
        {
            snapshot = _loggers.ToList();
        }

        foreach (var logger in snapshot.Where(x => x != null).OrderBy(x => x.LastLogWritten))
        {
            logger.Dispose();
        }
    }

    public IModuleLogger? GetLogger(Type type)
    {
        lock (_loggers)
        {
            return _loggers.FirstOrDefault(logger => logger.GetType() == type);
        }
    }

    public void AddLogger(ModuleLogger logger)
    {
        lock (_loggers)
        {
            _loggers.Add(logger);
        }
    }

    public void Dispose()
    {
        FlushAndDisposeAll();
    }
}