using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IModuleExecutor _moduleExecutor;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly ILogger<PipelineExecutor> _logger;
    private readonly IExceptionContainer _exceptionContainer;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleExecutor moduleExecutor,
        EngineCancellationToken engineCancellationToken,
        ILogger<PipelineExecutor> logger,
        IExceptionContainer exceptionContainer)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleExecutor = moduleExecutor;
        _engineCancellationToken = engineCancellationToken;
        _logger = logger;
        _exceptionContainer = exceptionContainer;
    }

    public async Task<PipelineSummary> ExecuteAsync(List<ModuleBase> runnableModules,
        OrganizedModules organizedModules)
    {
        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();

        Exception? exception;
        PipelineSummary pipelineSummary;
        try
        {
            await _moduleExecutor.ExecuteAsync(runnableModules);
        }
        finally
        {
            exception = await WaitForAlwaysRunModules(runnableModules);

            var end = DateTimeOffset.UtcNow;

            pipelineSummary = new PipelineSummary(organizedModules.AllModules, stopWatch.Elapsed, start, end);

            await _pipelineSetupExecutor.OnEndAsync(pipelineSummary);
        }
        
        _exceptionContainer.ThrowExceptions();

        if (exception != null)
        {
            throw exception;
        }
        
        return pipelineSummary;
    }

    private async Task<Exception?> WaitForAlwaysRunModules(IEnumerable<ModuleBase> runnableModules)
    {
        try
        {
            await Task.WhenAll(runnableModules.Where(m => m.ModuleRunType == ModuleRunType.AlwaysRun).Select(m => m.ExecutionTask));
        }
        catch (Exception? e)
        {
            _logger.LogWarning(e, "Error while waiting for Always Run modules");
            return e;
        }

        return null;
    }
}