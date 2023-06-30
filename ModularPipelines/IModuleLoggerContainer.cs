namespace ModularPipelines;

internal interface IModuleLoggerContainer
{
    void PrintAllLoggers();
    void AddLogger(ModuleLogger logger);
}