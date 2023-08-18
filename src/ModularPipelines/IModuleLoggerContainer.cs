using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines;

internal interface IModuleLoggerContainer
{
    void PrintAllLoggers();
    void AddLogger(ModuleLogger logger);

    bool TryGetModuleLogger(Type type, [NotNullWhen(true)] out ModuleLogger? moduleLogger);
}
