using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class PrintModuleOutputExecutor : IPrintModuleOutputExecutor
{
    private readonly IModuleLoggerContainer _moduleLoggerContainer;

    public PrintModuleOutputExecutor(IModuleLoggerContainer moduleLoggerContainer)
    {
        _moduleLoggerContainer = moduleLoggerContainer;
    }
    
    public async Task<IReadOnlyList<ModuleBase>> ExecuteAndPrintModuleOutput(Func<Task<IReadOnlyList<ModuleBase>>> executeDelegate)
    {
        try
        {
            return await executeDelegate();
        }
        finally
        {
            _moduleLoggerContainer.PrintAllLoggers();
        }
    }
}