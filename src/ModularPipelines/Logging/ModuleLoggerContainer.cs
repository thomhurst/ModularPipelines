namespace ModularPipelines.Logging;

internal class ModuleLoggerContainer : IModuleLoggerContainer
{
    private readonly List<ModuleLogger> _loggers = new();

    public void PrintAllLoggers()
    {
        foreach (var logger in _loggers.OrderBy(x => x.LastLogWritten).ToList())
        {
            logger.Dispose();
        }
    }

    public void AddLogger(ModuleLogger? logger)
    {
        if (logger is not null)
        {
            _loggers.Add(logger);
        }
    }
}
