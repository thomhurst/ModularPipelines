using System.Diagnostics;
using ModularPipelines.Logging;

namespace ModularPipelines.Engine.Executors;

[StackTraceHidden]
internal class PrintModuleOutputExecutor : IPrintModuleOutputExecutor
{
    private readonly IModuleLoggerContainer _moduleLoggerContainer;

    public PrintModuleOutputExecutor(IModuleLoggerContainer moduleLoggerContainer)
    {
        _moduleLoggerContainer = moduleLoggerContainer;
    }

    public void Dispose()
    {
        _moduleLoggerContainer.PrintAllLoggers();
    }
}