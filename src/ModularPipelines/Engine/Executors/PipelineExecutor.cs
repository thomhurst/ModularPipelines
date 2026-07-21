using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Executors;

internal class PipelineExecutor : IPipelineExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IModuleExecutor _moduleExecutor;
    private readonly ILogger<PipelineExecutor> _logger;
    private readonly IExceptionRethrowService _exceptionRethrowService;
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly IOptions<PipelineOptions> _options;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleExecutor moduleExecutor,
        ILogger<PipelineExecutor> logger,
        IExceptionRethrowService exceptionRethrowService,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IModuleResultRegistry resultRegistry,
        IMetricsCollector metricsCollector,
        IParallelLimitProvider parallelLimitProvider,
        IOptions<PipelineOptions> options)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleExecutor = moduleExecutor;
        _logger = logger;
        _exceptionRethrowService = exceptionRethrowService;
        _secondaryExceptionContainer = secondaryExceptionContainer;
        _resultRegistry = resultRegistry;
        _metricsCollector = metricsCollector;
        _parallelLimitProvider = parallelLimitProvider;
        _options = options;
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

            await _pipelineSetupExecutor.OnPipelineEndAsync(pipelineSummary).ConfigureAwait(false);
        }

        // Wait-for-all may return a failed summary when configured not to throw.
        // Fail-fast retains its existing behavior and always surfaces the original.
        if (_options.Value.ExecutionMode == ExecutionMode.StopOnFirstException
            || _options.Value.ThrowOnPipelineFailure)
        {
            _exceptionRethrowService.ThrowOriginalExceptionIfPresent();
        }

        _secondaryExceptionContainer.ThrowExceptions();

        return pipelineSummary;
    }
}
