namespace ModularPipelines;

internal class ModuleLoggerContainer : IModuleLoggerContainer
{
    private readonly List<ModuleLogger> _loggers = new();

    public void PrintAllLoggers()
    {
        foreach (var logger in _loggers.OrderBy(x => x.LastLogWritten))
        {
            logger.Dispose();
        }
    }

    public void AddLogger(ModuleLogger logger)
    {
        _loggers.Add(logger);
    }
}
