using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IModuleExecutor _moduleExecutor;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly ILogger<PipelineExecutor> _logger;
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleExecutor moduleExecutor,
        EngineCancellationToken engineCancellationToken,
        ILogger<PipelineExecutor> logger,
        ISecondaryExceptionContainer secondaryExceptionContainer)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleExecutor = moduleExecutor;
        _engineCancellationToken = engineCancellationToken;
        _logger = logger;
        _secondaryExceptionContainer = secondaryExceptionContainer;
    }

    public async Task<PipelineSummary> ExecuteAsync(List<IModule> runnableModules,
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

        // Check for original exception first with preserved stack trace
        if (_engineCancellationToken.OriginalExceptionDispatchInfo != null)
        {
            _engineCancellationToken.OriginalExceptionDispatchInfo.Throw();
        }

        _secondaryExceptionContainer.ThrowExceptions();

        if (exception != null)
        {
            throw exception;
        }

        return pipelineSummary;
    }

    private async Task<Exception?> WaitForAlwaysRunModules(IEnumerable<IModule> runnableModules)
    {
        // v3.0: AlwaysRun modules are handled by the scheduler in ModuleExecutor
        // This method is kept for backward compatibility but is now a no-op
        return await Task.FromResult<Exception?>(null);
    }
}
