using System.Diagnostics;
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
    private readonly IPipelineSummaryFactory _pipelineSummaryFactory;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IThreadPoolConfigurator _threadPoolConfigurator;
    private readonly IExceptionRethrowService _exceptionRethrowService;
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogger<ExecutionOrchestrator> _logger;

    private readonly object _lock = new();

    private bool _hasRun;

    public ExecutionOrchestrator(
        IPipelineInitializer pipelineInitializer,
        IModuleDisposeExecutor moduleDisposeExecutor,
        IPipelineExecutor pipelineExecutor,
        IPipelineOutputCoordinator outputCoordinator,
        IIgnoredModuleResultRegistrar ignoredModuleResultRegistrar,
        IPipelineSummaryFactory pipelineSummaryFactory,
        EngineCancellationToken engineCancellationToken,
        IThreadPoolConfigurator threadPoolConfigurator,
        IExceptionRethrowService exceptionRethrowService,
        IOptions<PipelineOptions> options,
        ILogger<ExecutionOrchestrator> logger)
    {
        _pipelineInitializer = pipelineInitializer;
        _moduleDisposeExecutor = moduleDisposeExecutor;
        _pipelineExecutor = pipelineExecutor;
        _outputCoordinator = outputCoordinator;
        _ignoredModuleResultRegistrar = ignoredModuleResultRegistrar;
        _pipelineSummaryFactory = pipelineSummaryFactory;
        _engineCancellationToken = engineCancellationToken;
        _threadPoolConfigurator = threadPoolConfigurator;
        _exceptionRethrowService = exceptionRethrowService;
        _options = options;
        _logger = logger;
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

        cancellationToken.Register(() => _engineCancellationToken.CancelWithReason("The user's cancellation token passed into the pipeline was cancelled."));

        _threadPoolConfigurator.Configure();

        var organizedModules = await _pipelineInitializer.Initialize().ConfigureAwait(false);

        // Register skipped results for ignored modules (via Category/RunCondition)
        await _ignoredModuleResultRegistrar.RegisterIgnoredModuleResultsAsync(organizedModules.IgnoredModules).ConfigureAwait(false);

        var runnableModules = organizedModules.RunnableModules.Select(x => (IModule) x.Module).ToList();

        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();

        try
        {
            var result = await ExecutePipeline(runnableModules, organizedModules).ConfigureAwait(false);

            return await OnEnd(organizedModules, stopWatch, start, result).ConfigureAwait(false);
        }
        catch
        {
            await OnEnd(organizedModules, stopWatch, start, null).ConfigureAwait(false);
            throw;
        }
    }

    private async Task<PipelineSummary> OnEnd(OrganizedModules organizedModules, Stopwatch stopWatch, DateTimeOffset start,
        PipelineSummary? pipelineSummary)
    {
        var end = DateTimeOffset.UtcNow;
        pipelineSummary ??= _pipelineSummaryFactory.Create(organizedModules.AllModules, stopWatch.Elapsed, start, end);

        _outputCoordinator.PrintResults(pipelineSummary);

        await System.Console.Out.FlushAsync().ConfigureAwait(false);

        // Flush any buffered exceptions after the results table has been printed
        _outputCoordinator.FlushExceptions();

        // Check for original exception before logging cancellation reason
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

        // Throw exception if pipeline failed - this ensures non-zero exit code in CI
        if (pipelineSummary.Status == Status.Failed && _options.Value.ThrowOnPipelineFailure)
        {
            var failedModules = pipelineSummary.GetFailedModuleResults()
                .Select(r => r.ModuleName)
                .ToList();

            throw new PipelineFailedException(pipelineSummary, failedModules);
        }

        return pipelineSummary;
    }

    private async Task<PipelineSummary> ExecutePipeline(List<IModule> runnableModules, OrganizedModules organizedModules)
    {
        // Dispose and flush on scope leave - So including success or if an exception is thrown
        var moduleDisposeExecutor = _moduleDisposeExecutor;
        await using (moduleDisposeExecutor.ConfigureAwait(false))
        {
            await using var outputScope = await _outputCoordinator.InitializeAsync().ConfigureAwait(false);
            return await _pipelineExecutor.ExecuteAsync(runnableModules, organizedModules).ConfigureAwait(false);
        }
    }
}
