namespace ModularPipelines.Logging;

internal interface IModuleLoggerContainer
{
    void PrintAllLoggers();

    IModuleLogger? GetLogger(Type type);

    void AddLogger(ModuleLogger logger);
}