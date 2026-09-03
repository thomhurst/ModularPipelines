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
    private readonly IExecutionBackend _executionBackend;
    private readonly IExecutionBackendContext _executionBackendContext;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly ILogger<PipelineExecutor> _logger;
    private readonly IExceptionRethrowService _exceptionRethrowService;
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IPipelineSummaryFactory _pipelineSummaryFactory;
    private readonly IOptions<PipelineOptions> _options;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IExecutionBackend executionBackend,
        IExecutionBackendContext executionBackendContext,
        EngineCancellationToken engineCancellationToken,
        ILogger<PipelineExecutor> logger,
        IExceptionRethrowService exceptionRethrowService,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IPipelineSummaryFactory pipelineSummaryFactory,
        IOptions<PipelineOptions> options)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _executionBackend = executionBackend;
        _executionBackendContext = executionBackendContext;
        _engineCancellationToken = engineCancellationToken;
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
            var results = await _executionBackend.ExecuteAsync(
                    runnableModules,
                    _executionBackendContext,
                    _engineCancellationToken.Token)
                .ConfigureAwait(false);
            ApplyBackendResults(runnableModules, results);
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

    private void ApplyBackendResults(
        IReadOnlyList<IModule> modules,
        IReadOnlyList<IModuleResult> results)
    {
        foreach (var result in results)
        {
            var matchingModules = modules
                .Where(module => result.TypeName is not null
                    ? string.Equals(module.GetType().FullName, result.TypeName, StringComparison.Ordinal)
                    : string.Equals(module.GetType().Name, result.Name, StringComparison.Ordinal))
                .ToArray();
            if (matchingModules.Length != 1)
            {
                throw new InvalidOperationException(
                    $"Execution backend returned result '{result.Name}' with type '{result.TypeName}', "
                    + $"which matched {matchingModules.Length} planned modules.");
            }

            _executionBackendContext.TryApplyResult(matchingModules[0], result);
        }

        var incompleteModules = modules
            .Where(module => !module.AsInternal().ResultTask.IsCompleted)
            .Select(module => module.GetType().FullName ?? module.GetType().Name)
            .ToArray();
        if (incompleteModules.Length > 0)
        {
            throw new InvalidOperationException(
                "Execution backend completed without results for: "
                + string.Join(", ", incompleteModules));
        }
    }
}
