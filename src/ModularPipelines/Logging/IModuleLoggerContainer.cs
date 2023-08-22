namespace ModularPipelines.Logging;

internal interface IModuleLoggerContainer
{
    void PrintAllLoggers();
    void AddLogger(ModuleLogger logger);
}
