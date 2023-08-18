using System.Diagnostics.CodeAnalysis;

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
    
    public bool TryGetModuleLogger(Type type, [NotNullWhen(true)] out ModuleLogger? moduleLogger)
    {
        moduleLogger = _loggers.FirstOrDefault(ml => ml.GetType() == type);
        return moduleLogger != null;
    }
}
