using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
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
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IParallelLimitProvider _parallelLimitProvider;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleExecutor moduleExecutor,
        EngineCancellationToken engineCancellationToken,
        ILogger<PipelineExecutor> logger,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IModuleResultRegistry resultRegistry,
        IMetricsCollector metricsCollector,
        IParallelLimitProvider parallelLimitProvider)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleExecutor = moduleExecutor;
        _engineCancellationToken = engineCancellationToken;
        _logger = logger;
        _secondaryExceptionContainer = secondaryExceptionContainer;
        _resultRegistry = resultRegistry;
        _metricsCollector = metricsCollector;
        _parallelLimitProvider = parallelLimitProvider;
    }

    public async Task<PipelineSummary> ExecuteAsync(List<IModule> runnableModules,
        OrganizedModules organizedModules)
    {
        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();

        PipelineSummary pipelineSummary;
        try
        {
            // ModuleExecutor handles waiting for AlwaysRun modules internally
            await _moduleExecutor.ExecuteAsync(runnableModules).ConfigureAwait(false);
        }
        finally
        {
            var end = DateTimeOffset.UtcNow;

            pipelineSummary = new PipelineSummary(organizedModules.AllModules, stopWatch.Elapsed, start, end, _resultRegistry, _metricsCollector, _parallelLimitProvider.GetMaxDegreeOfParallelism());

            await _pipelineSetupExecutor.OnEndAsync(pipelineSummary).ConfigureAwait(false);
        }

        // Check for original exception first with preserved stack trace
        if (_engineCancellationToken.OriginalExceptionDispatchInfo != null)
        {
            _engineCancellationToken.OriginalExceptionDispatchInfo.Throw();
        }

        _secondaryExceptionContainer.ThrowExceptions();

        return pipelineSummary;
    }
}