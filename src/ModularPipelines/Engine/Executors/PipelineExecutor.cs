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
    private readonly IExecutionBackendContextFactory _executionBackendContextFactory;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly ILogger<PipelineExecutor> _logger;
    private readonly IExceptionRethrowService _exceptionRethrowService;
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IPipelineSummaryFactory _pipelineSummaryFactory;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IMetricsCollector? _metricsCollector;

    public PipelineExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IExecutionBackend executionBackend,
        IExecutionBackendContext executionBackendContext,
        IExecutionBackendContextFactory executionBackendContextFactory,
        EngineCancellationToken engineCancellationToken,
        ILogger<PipelineExecutor> logger,
        IExceptionRethrowService exceptionRethrowService,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IPipelineSummaryFactory pipelineSummaryFactory,
        IOptions<PipelineOptions> options,
        IMetricsCollector? metricsCollector = null)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _executionBackend = executionBackend;
        _executionBackendContext = executionBackendContext;
        _executionBackendContextFactory = executionBackendContextFactory;
        _engineCancellationToken = engineCancellationToken;
        _logger = logger;
        _exceptionRethrowService = exceptionRethrowService;
        _secondaryExceptionContainer = secondaryExceptionContainer;
        _pipelineSummaryFactory = pipelineSummaryFactory;
        _options = options;
        _metricsCollector = metricsCollector;
    }

    public async Task<PipelineSummary> ExecuteAsync(List<IModule> runnableModules,
        OrganizedModules organizedModules)
    {
        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();

        PipelineSummary pipelineSummary;
        List<IModule> executedModules = [];
        try
        {
            var estimatedDurations = organizedModules.RunnableModules.ToDictionary(
                runnable => runnable.Module.GetType(),
                runnable => runnable.EstimatedDuration);
            // Only custom backends need shared dispatch state; the built-in backend owns its scheduler directly.
            var context = _executionBackend is ModuleExecutor
                ? null
                : _executionBackendContextFactory.Create(_executionBackendContext, runnableModules, estimatedDurations, _engineCancellationToken);
            try
            {
                var results = await _executionBackend.ExecuteAsync(
                        runnableModules,
                        estimatedDurations,
                        context ?? _executionBackendContext,
                        _engineCancellationToken.Token)
                    .ConfigureAwait(false);
                executedModules = ApplyBackendResults(runnableModules, results);
            }
            finally
            {
                if (context is IAsyncDisposable contextLifetime)
                {
                    await contextLifetime.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
        finally
        {
            var end = DateTimeOffset.UtcNow;

            // A backend that runs only a claimed subset (a distributed worker) summarizes the
            // modules it executed; the coordinator reports the whole plan.
            pipelineSummary = _pipelineSummaryFactory.Create(
                _executionBackend.OwnsEntirePlan ? organizedModules.AllModules : executedModules,
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

    private List<IModule> ApplyBackendResults(
        IReadOnlyList<IModule> modules,
        IReadOnlyList<IModuleResult> results)
    {
        var executedModules = new List<IModule>(results.Count);
        foreach (var result in results)
        {
            var matchingModule = FindModuleOwningResult(modules, result)
                                 ?? FindModuleByTypeName(modules, result);
            executedModules.Add(matchingModule);
            if (!_executionBackend.OwnsEntirePlan)
            {
                RecordClaimedModuleMetrics(matchingModule, result);
            }

            if (_executionBackendContext.TryApplyResult(matchingModule, result))
            {
                continue;
            }

            var resultTask = matchingModule.AsInternal().ResultTask;
            if (!resultTask.IsCompletedSuccessfully
                || !ReferenceEquals(resultTask.Result, result))
            {
                throw new InvalidOperationException(
                    $"Execution backend returned a conflicting result for module '{matchingModule.GetType().FullName}'.");
            }
        }

        if (!_executionBackend.OwnsEntirePlan)
        {
            RecordClaimedModuleConcurrency(results);
            return executedModules;
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

        return executedModules;
    }

    /// <summary>
    /// Records a claimed module's reported execution window and status. Backends that run only a
    /// claimed subset bypass the scheduler, which otherwise records these metrics.
    /// </summary>
    private void RecordClaimedModuleMetrics(IModule module, IModuleResult result)
    {
        if (_metricsCollector is null || result.StartTime == default)
        {
            return;
        }

        var moduleType = module.GetType();
        var endTime = result.EndTime < result.StartTime ? result.StartTime : result.EndTime;
        _metricsCollector.RecordModuleStarted(moduleType, result.StartTime);
        _metricsCollector.RecordModuleCompleted(
            moduleType,
            endTime,
            result.ExceptionOrDefault is null,
            result.Status == ModuleStatus.Skipped,
            result.Status);
    }

    private void RecordClaimedModuleConcurrency(IReadOnlyList<IModuleResult> results)
    {
        if (_metricsCollector is null)
        {
            return;
        }

        // Ends sort before starts at the same instant so back-to-back modules do not overlap.
        var events = results
            .Where(static result => result.StartTime != default)
            .SelectMany(static result => new[]
            {
                (Time: result.StartTime, Delta: 1),
                (Time: result.EndTime < result.StartTime ? result.StartTime : result.EndTime, Delta: -1),
            })
            .OrderBy(static change => change.Time)
            .ThenBy(static change => change.Delta);
        var concurrency = 0;
        foreach (var change in events)
        {
            concurrency += change.Delta;
            _metricsCollector.RecordConcurrencySnapshot(concurrency, change.Time);
        }
    }

    /// <summary>
    /// Finds the planned module whose own result instance the backend handed back. In-process
    /// backends return the modules' result objects, so identity resolves the module even when
    /// several planned modules share a type name across assemblies.
    /// </summary>
    private static IModule? FindModuleOwningResult(
        IReadOnlyList<IModule> modules,
        IModuleResult result) =>
        modules.FirstOrDefault(module =>
            module.AsInternal().ResultTask is { IsCompletedSuccessfully: true } resultTask
            && ReferenceEquals(resultTask.Result, result));

    /// <summary>
    /// Resolves a foreign (for example deserialized) backend result to the single planned
    /// module declaring its fully qualified type name.
    /// </summary>
    private static IModule FindModuleByTypeName(
        IReadOnlyList<IModule> modules,
        IModuleResult result)
    {
        if (string.IsNullOrWhiteSpace(result.TypeName))
        {
            throw new InvalidOperationException(
                $"Execution backend result '{result.Name}' must provide a fully qualified TypeName.");
        }

        var matchingModules = modules
            .Where(module => string.Equals(
                module.GetType().FullName,
                result.TypeName,
                StringComparison.Ordinal))
            .ToArray();
        if (matchingModules.Length != 1)
        {
            throw new InvalidOperationException(
                $"Execution backend returned result '{result.Name}' with type '{result.TypeName}', "
                + $"which matched {matchingModules.Length} planned modules.");
        }

        return matchingModules[0];
    }
}
