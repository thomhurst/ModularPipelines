using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class ExecutionOrchestrator : IExecutionOrchestrator
{
    private readonly IPipelineInitializer _pipelineInitializer;
    private readonly IModuleDisposeExecutor _moduleDisposeExecutor;
    private readonly IPrintModuleOutputExecutor _printModuleOutputExecutor;
    private readonly IPrintProgressExecutor _printProgressExecutor;
    private readonly IPipelineExecutor _pipelineExecutor;

    public ExecutionOrchestrator(IPipelineInitializer pipelineInitializer,
        IModuleDisposeExecutor moduleDisposeExecutor,
        IPrintModuleOutputExecutor printModuleOutputExecutor,
        IPrintProgressExecutor printProgressExecutor,
        IPipelineExecutor pipelineExecutor)
    {
        _pipelineInitializer = pipelineInitializer;
        _moduleDisposeExecutor = moduleDisposeExecutor;
        _printModuleOutputExecutor = printModuleOutputExecutor;
        _printProgressExecutor = printProgressExecutor;
        _pipelineExecutor = pipelineExecutor;
    }

    public async Task<IReadOnlyList<ModuleBase>> ExecuteAsync()
    {
        var organizedModules = await _pipelineInitializer.Initialize();
        
        var runnableModules = organizedModules.RunnableModules.Select(x => x.Module).ToList();

        return await _moduleDisposeExecutor.ExecuteAndDispose(runnableModules, async () =>
        {
            return await _printModuleOutputExecutor.ExecuteAndPrintModuleOutput(async () =>
            {
                return await _printProgressExecutor.ExecuteWithProgress(organizedModules, async () =>
                {
                    return await _pipelineExecutor.ExecuteAsync(runnableModules, organizedModules);
                });
            });
        });
    }
}