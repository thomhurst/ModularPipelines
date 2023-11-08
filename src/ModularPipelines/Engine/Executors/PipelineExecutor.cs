using System.Diagnostics;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IModuleExecutor _moduleExecutor;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleExecutor moduleExecutor)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleExecutor = moduleExecutor;
    }

    public async Task<PipelineSummary> ExecuteAsync(List<ModuleBase> runnableModules,
        OrganizedModules organizedModules)
    {
        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();

        PipelineSummary pipelineSummary;
        try
        {
            await _moduleExecutor.ExecuteAsync(runnableModules);
        }
        catch
        {
            // Give time for the console to update modules to Failed
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            throw;
        }
        finally
        {
            await WaitForAlwaysRunModules(runnableModules);

            var end = DateTimeOffset.UtcNow;

            pipelineSummary = new PipelineSummary(organizedModules.AllModules, stopWatch.Elapsed, start, end);

            await _pipelineSetupExecutor.OnEndAsync(pipelineSummary);
        }

        return pipelineSummary;
    }

    private async Task WaitForAlwaysRunModules(IEnumerable<ModuleBase> runnableModules)
    {
        try
        {
            await Task.WhenAll(runnableModules.Where(m => m.ModuleRunType == ModuleRunType.AlwaysRun).Select(m => m.WaitTask));
        }
        catch
        {
            // Ignored
        }
    }
}
