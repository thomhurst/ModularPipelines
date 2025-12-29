using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class ExecutionOrchestrator : IExecutionOrchestrator
{
    private readonly IPipelineInitializer _pipelineInitializer;
    private readonly IModuleDisposeExecutor _moduleDisposeExecutor;
    private readonly IPipelineExecutor _pipelineExecutor;
    private readonly IPipelineOutputCoordinator _outputCoordinator;
    private readonly IIgnoredModuleResultRegistrar _ignoredModuleResultRegistrar;
    private readonly IPipelineSummaryFactory _pipelineSummaryFactory;
    private readonly EngineCancellationToken _engineCancellationToken;
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
        ILogger<ExecutionOrchestrator> logger)
    {
        _pipelineInitializer = pipelineInitializer;
        _moduleDisposeExecutor = moduleDisposeExecutor;
        _pipelineExecutor = pipelineExecutor;
        _outputCoordinator = outputCoordinator;
        _ignoredModuleResultRegistrar = ignoredModuleResultRegistrar;
        _pipelineSummaryFactory = pipelineSummaryFactory;
        _engineCancellationToken = engineCancellationToken;
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
            if (_engineCancellationToken.OriginalExceptionDispatchInfo != null)
            {
                _engineCancellationToken.OriginalExceptionDispatchInfo.Throw();
            }

            // Otherwise throw the cancellation exception
            throw;
        }
        finally
        {
            _outputCoordinator.WriteLogs();
        }
    }

    private void ConfigureThreadPool()
    {
        // Get current thread pool settings
        ThreadPool.GetMinThreads(out var currentWorkerThreads, out var currentCompletionPortThreads);

        // Calculate desired MinThreads to ensure adequate parallelism
        // Use ProcessorCount * 4 to provide headroom for parallel module execution
        var desiredMinThreads = Environment.ProcessorCount * 4;

        // Only increase if current value is lower
        if (currentWorkerThreads < desiredMinThreads)
        {
            ThreadPool.SetMinThreads(desiredMinThreads, currentCompletionPortThreads);
            _logger.LogDebug(
                "Configured ThreadPool MinThreads from {OldValue} to {NewValue} (ProcessorCount: {ProcessorCount})",
                currentWorkerThreads,
                desiredMinThreads,
                Environment.ProcessorCount);
        }
        else
        {
            _logger.LogDebug(
                "ThreadPool MinThreads already adequate: {CurrentValue} (ProcessorCount: {ProcessorCount})",
                currentWorkerThreads,
                Environment.ProcessorCount);
        }
    }

    private async Task<PipelineSummary> ExecuteInternal(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (_hasRun)
            {
                throw new Exception("This pipeline has already been triggerred.");
            }

            _hasRun = true;
        }

        cancellationToken.Register(() => _engineCancellationToken.CancelWithReason("The user's cancellation token passed into the pipeline was cancelled."));

        ConfigureThreadPool();

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

        await Console.Out.FlushAsync().ConfigureAwait(false);

        // Flush any buffered exceptions after the results table has been printed
        _outputCoordinator.FlushExceptions();

        // Check for original exception before logging cancellation reason
        if (_engineCancellationToken.OriginalException != null)
        {
            _logger.LogInformation("{Header} {ExceptionType}",
                MarkupFormatter.FormatColoredHeader("Pipeline Failed", "red"),
                _engineCancellationToken.OriginalException.GetType().Name);
        }
        else if (!string.IsNullOrEmpty(_engineCancellationToken.Reason))
        {
            _logger.LogInformation("{Header} {Reason}",
                MarkupFormatter.FormatColoredHeader("Cancellation", "yellow"),
                _engineCancellationToken.Reason);
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
