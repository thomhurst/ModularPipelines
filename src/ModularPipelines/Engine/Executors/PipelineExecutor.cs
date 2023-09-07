using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IModuleExecutor _moduleExecutor;
    private readonly EngineCancellationToken _engineCancellationToken;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleExecutor moduleExecutor,
        EngineCancellationToken engineCancellationToken)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleExecutor = moduleExecutor;
        _engineCancellationToken = engineCancellationToken;
    }

    public async Task<IReadOnlyList<ModuleBase>> ExecuteAsync(List<ModuleBase> runnableModules,
        OrganizedModules organizedModules)
    {
        try
        {
            await _moduleExecutor.ExecuteAsync(runnableModules);
        }
        catch
        {
            // Give time for the console to update modules to Failed
            await Task.Delay(TimeSpan.FromSeconds(1));
            throw;
        }
        finally
        {
            await WaitForAlwaysRunModules(runnableModules);

            await _pipelineSetupExecutor.OnEndAsync(organizedModules.AllModules);
        }

        return organizedModules.AllModules;
    }

    private async Task WaitForAlwaysRunModules(IEnumerable<ModuleBase> runnableModules)
    {
        try
        {
            await Task.WhenAll(runnableModules.Where(m => m.ModuleRunType == ModuleRunType.AlwaysRun).Select(m => m.ResultTaskInternal));
        }
        catch
        {
            // Ignored
        }
    }
}
