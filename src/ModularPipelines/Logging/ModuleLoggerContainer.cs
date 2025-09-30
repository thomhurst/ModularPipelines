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
internal class ModuleLoggerContainer : IModuleLoggerContainer
{
    private readonly List<ModuleLogger> _loggers = new();

    public void FlushAndDisposeAll()
    {
        foreach (var logger in _loggers.OrderBy(x => x.LastLogWritten).ToList())
        {
            logger.Dispose();
        }
    }

    public IModuleLogger? GetLogger(Type type)
    {
        return _loggers.FirstOrDefault(logger => logger.GetType() == type);
    }

    public void AddLogger(ModuleLogger logger)
    {
        _loggers.Add(logger);
    }
}