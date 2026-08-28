using System.Diagnostics;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Orchestrates the execution of the entire pipeline lifecycle.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> The <see cref="ExecuteAsync"/> method can only be called once per instance.
/// Subsequent calls will throw <see cref="InvalidOperationException"/>.
/// </para>
/// <para>
/// <b>Synchronization Strategy:</b> Uses a simple lock to ensure single execution.
/// This is a defensive pattern to catch programming errors where a pipeline is
/// accidentally executed multiple times. The lock protects the check-then-set
/// pattern for the <c>_hasRun</c> flag.
/// </para>
/// </remarks>
internal class ExecutionOrchestrator : IExecutionOrchestrator
{
    private readonly IPipelineInitializer _pipelineInitializer;
    private readonly IModuleDisposeExecutor _moduleDisposeExecutor;
    private readonly IPipelineExecutor _pipelineExecutor;
    private readonly IPipelineOutputCoordinator _outputCoordinator;
    private readonly IIgnoredModuleResultRegistrar _ignoredModuleResultRegistrar;
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IPipelineSummaryFactory _pipelineSummaryFactory;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IThreadPoolConfigurator _threadPoolConfigurator;
    private readonly IExceptionRethrowService _exceptionRethrowService;
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogger<ExecutionOrchestrator> _logger;
    private readonly IRunReportService _runReportService;

    private readonly object _lock = new();

    private bool _hasRun;

    public ExecutionOrchestrator(
        IPipelineInitializer pipelineInitializer,
        IModuleDisposeExecutor moduleDisposeExecutor,
        IPipelineExecutor pipelineExecutor,
        IPipelineOutputCoordinator outputCoordinator,
        IIgnoredModuleResultRegistrar ignoredModuleResultRegistrar,
        IModuleResultRegistry resultRegistry,
        IPipelineSummaryFactory pipelineSummaryFactory,
        EngineCancellationToken engineCancellationToken,
        IThreadPoolConfigurator threadPoolConfigurator,
        IExceptionRethrowService exceptionRethrowService,
        IOptions<PipelineOptions> options,
        ILogger<ExecutionOrchestrator> logger,
        IRunReportService runReportService)
    {
        _pipelineInitializer = pipelineInitializer;
        _moduleDisposeExecutor = moduleDisposeExecutor;
        _pipelineExecutor = pipelineExecutor;
        _outputCoordinator = outputCoordinator;
        _ignoredModuleResultRegistrar = ignoredModuleResultRegistrar;
        _resultRegistry = resultRegistry;
        _pipelineSummaryFactory = pipelineSummaryFactory;
        _engineCancellationToken = engineCancellationToken;
        _threadPoolConfigurator = threadPoolConfigurator;
        _exceptionRethrowService = exceptionRethrowService;
        _options = options;
        _logger = logger;
        _runReportService = runReportService;
    }

    public async Task<PipelineSummary> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await ExecuteInternal(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is PipelineCancelledException or TaskCanceledException or OperationCanceledException)
        {
            // Check if we have an original exception stored with preserved stack trace
            _exceptionRethrowService.ThrowOriginalExceptionIfPresent();

            // Otherwise throw the cancellation exception
            throw;
        }
        finally
        {
            _outputCoordinator.WriteLogs();
        }
    }

    private async Task<PipelineSummary> ExecuteInternal(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (_hasRun)
            {
                throw new InvalidOperationException("This pipeline has already been triggered.");
            }

            _hasRun = true;
        }

        using var cancellationRegistration = cancellationToken.Register(
            () => _engineCancellationToken.CancelWithReason(
                "The user's cancellation token passed into the pipeline was cancelled."));

        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();
        IReadOnlyList<IModule> allModules = [];
        PipelineSummary? summary = null;
        Exception? caughtException = null;
        try
        {
            allModules = _pipelineInitializer.RegisteredModules ?? [];
            _threadPoolConfigurator.Configure();

            var organizedModules = await _pipelineInitializer.Initialize(cancellationToken).ConfigureAwait(false);
            allModules = organizedModules.AllModules;

            // Resolve ignored modules against history before cascading required dependencies.
            organizedModules = await _ignoredModuleResultRegistrar
                .RegisterIgnoredModuleResultsAsync(organizedModules)
                .ConfigureAwait(false);
            allModules = organizedModules.AllModules;

            var runnableModules = organizedModules.RunnableModules
                .Select(x => (IModule) x.Module)
                .Concat(organizedModules.IgnoredModules
                    .Select(ignoredModule => ignoredModule.Module)
                    .Where(module => _resultRegistry.GetResult(module.GetType())?.Status == ModuleStatus.RestoredFromHistory))
                .ToList();

            summary = await ExecutePipeline(runnableModules, organizedModules).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        // Step 2: Always print summary (exactly once)
        summary = await PrintSummary(
                allModules,
                stopWatch,
                start,
                summary,
                caughtException,
                _engineCancellationToken.NonFailureCancellationToken)
            .ConfigureAwait(false);

        // Step 3: Handle exceptions - rethrow original if present, otherwise check for pipeline failure
        if (caughtException != null)
        {
            ExceptionDispatchInfo.Capture(caughtException).Throw();
        }

        if (summary.Status == ModuleStatus.Failed && _options.Value.ThrowOnPipelineFailure)
        {
            var failedModules = summary.Results
                .Where(result => result.ExceptionOrDefault is not null)
                .Select(r => r.ModuleName)
                .ToList();

            throw new PipelineFailedException(summary, failedModules);
        }

        return summary;
    }

    /// <summary>
    /// Prints the pipeline summary and flushes output. Does not throw exceptions.
    /// </summary>
    private async Task<PipelineSummary> PrintSummary(
        IReadOnlyList<IModule> modules,
        Stopwatch stopWatch,
        DateTimeOffset start,
        PipelineSummary? existingSummary,
        Exception? pipelineException,
        CancellationToken cancellationToken)
    {
        var end = DateTimeOffset.UtcNow;
        var totalDuration = stopWatch.Elapsed;
        var summary = existingSummary is null
            ? _pipelineSummaryFactory.Create(modules, totalDuration, start, end)
            : new PipelineSummary(
                existingSummary.Modules,
                existingSummary.Results,
                totalDuration,
                start,
                end,
                RecomputeDurationMetrics(existingSummary.Metrics, totalDuration),
                existingSummary.ModuleTimelines)
            {
                StatusOverride = existingSummary.StatusOverride,
            };

        await FlushConsoleBestEffortAsync().ConfigureAwait(false);

        summary = summary with
        {
            RunReport = await _runReportService.CompleteAsync(
                    summary,
                    GetPipelineReportException(summary, pipelineException),
                    cancellationToken)
                .ConfigureAwait(false),
            StatusOverride = pipelineException is null ? summary.StatusOverride : ModuleStatus.Failed,
        };

        _outputCoordinator.PrintResults(summary);

        await FlushConsoleBestEffortAsync().ConfigureAwait(false);

        // Flush any buffered exceptions after the results table has been printed
        _outputCoordinator.FlushExceptions();

        // Log cancellation/failure info (informational only)
        if (_engineCancellationToken.OriginalException != null)
        {
            _logger.LogInformation("Pipeline Failed: {ExceptionType}",
                _engineCancellationToken.OriginalException.GetType().Name);
        }
        else if (!string.IsNullOrEmpty(_engineCancellationToken.Reason))
        {
            _logger.LogInformation("Cancellation: {Reason}",
                _engineCancellationToken.Reason);
        }

        return summary;
    }

    private async Task FlushConsoleBestEffortAsync()
    {
        try
        {
            await _outputCoordinator.FlushConsoleAsync().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Could not flush pipeline console output");
        }
    }

    private static Exception? GetPipelineReportException(
        PipelineSummary summary,
        Exception? pipelineException)
    {
        if (pipelineException is null)
        {
            return null;
        }

        var moduleExceptions = summary.Results
            .Select(result => result.ExceptionOrDefault)
            .OfType<Exception>()
            .ToArray();
        return RemoveModuleExceptionBranches(pipelineException, moduleExceptions);
    }

    private static Exception? RemoveModuleExceptionBranches(
        Exception exception,
        IReadOnlyCollection<Exception> moduleExceptions)
    {
        if (moduleExceptions.Any(moduleException => ReferenceEquals(exception, moduleException)))
        {
            return null;
        }

        if (exception is AggregateException aggregateException)
        {
            var remainingExceptions = aggregateException.InnerExceptions
                .Select(innerException => RemoveModuleExceptionBranches(innerException, moduleExceptions))
                .OfType<Exception>()
                .ToArray();
            if (remainingExceptions.Length == aggregateException.InnerExceptions.Count)
            {
                return exception;
            }

            return remainingExceptions.Length == 0
                ? null
                : new FilteredRunReportAggregateException(
                    aggregateException,
                    remainingExceptions);
        }

        if (exception.InnerException is not { } innerException)
        {
            return exception;
        }

        var remainingInnerException = RemoveModuleExceptionBranches(innerException, moduleExceptions);
        if (remainingInnerException is null)
        {
            return exception is ModuleFailedException
                ? null
                : new FilteredRunReportException(exception, innerException: null);
        }

        return ReferenceEquals(remainingInnerException, innerException)
            ? exception
            : new FilteredRunReportException(exception, remainingInnerException);
    }

    private static PipelineMetrics? RecomputeDurationMetrics(
        PipelineMetrics? metrics,
        TimeSpan wallClockDuration)
    {
        if (metrics is null)
        {
            return null;
        }

        var parallelismFactor = wallClockDuration.TotalMilliseconds > 0
            ? metrics.TotalModuleExecutionTime.TotalMilliseconds / wallClockDuration.TotalMilliseconds
            : 1.0;
        return metrics with
        {
            WallClockDuration = wallClockDuration,
            ParallelismFactor = Math.Round(parallelismFactor, 2),
        };
    }

    private async Task<PipelineSummary> ExecutePipeline(List<IModule> runnableModules, OrganizedModules organizedModules)
    {
        // Dispose and flush on scope leave - So including success or if an exception is thrown
        var moduleDisposeExecutor = _moduleDisposeExecutor;
        await using (moduleDisposeExecutor.ConfigureAwait(false))
        {
            var outputScope = await _outputCoordinator.InitializeAsync().ConfigureAwait(false);
            Exception? pipelineException = null;

            try
            {
                return await _pipelineExecutor.ExecuteAsync(runnableModules, organizedModules).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                pipelineException = exception;
                throw;
            }
            finally
            {
                try
                {
                    await outputScope.DisposeAsync().ConfigureAwait(false);
                }
                catch (Exception teardownException) when (pipelineException is not null)
                {
                    try
                    {
                        _logger.LogError(
                            teardownException,
                            "Pipeline output teardown failed while the pipeline was already failing with {ExceptionType}.",
                            pipelineException.GetType().Name);
                    }
                    catch
                    {
                        // Diagnostic providers must not replace the primary pipeline failure.
                    }
                }
            }
        }
    }
}
