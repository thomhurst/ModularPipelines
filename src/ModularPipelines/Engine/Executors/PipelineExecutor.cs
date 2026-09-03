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
    private readonly IPipelineSummaryFactory _pipelineSummaryFactory;
    private readonly IOptions<PipelineOptions> _options;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IModuleExecutor moduleExecutor,
        ILogger<PipelineExecutor> logger,
        IExceptionRethrowService exceptionRethrowService,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IPipelineSummaryFactory pipelineSummaryFactory,
        IOptions<PipelineOptions> options)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _moduleExecutor = moduleExecutor;
        _logger = logger;
        _exceptionRethrowService = exceptionRethrowService;
        _secondaryExceptionContainer = secondaryExceptionContainer;
        _pipelineSummaryFactory = pipelineSummaryFactory;
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
            var estimatedDurations = organizedModules.RunnableModules.ToDictionary(
                runnable => runnable.Module.GetType(),
                runnable => runnable.EstimatedDuration);
            await _moduleExecutor.ExecuteAsync(runnableModules, estimatedDurations).ConfigureAwait(false);
        }
        finally
        {
            var end = DateTimeOffset.UtcNow;

            pipelineSummary = _pipelineSummaryFactory.Create(
                organizedModules.AllModules,
                stopWatch.Elapsed,
                start,
                end);

            await _pipelineSetupExecutor.OnPipelineEndAsync(pipelineSummary).ConfigureAwait(false);
        }

        // Wait-for-all may return a failed summary when configured not to throw.
        // Fail-fast retains its existing behavior and always surfaces the original.
        if (_options.Value.FailureMode == FailureMode.FailFast
            || _options.Value.ThrowOnPipelineFailure)
        {
            _exceptionRethrowService.ThrowOriginalExceptionIfPresent();
            _secondaryExceptionContainer.ThrowExceptions();
        }

        return pipelineSummary;
    }
}
