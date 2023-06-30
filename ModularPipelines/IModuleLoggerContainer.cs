using Microsoft.Extensions.Logging;

namespace ModularPipelines;

internal interface IModuleLoggerContainer
{
    void PrintAllLoggers();
    void AddLogger(ModuleLogger logger);
}