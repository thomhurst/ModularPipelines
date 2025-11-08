using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal class ExecutionOrchestrator : IExecutionOrchestrator
{
    private readonly IPipelineInitializer _pipelineInitializer;
    private readonly IModuleDisposeExecutor _moduleDisposeExecutor;
    private readonly IPrintModuleOutputExecutor _printModuleOutputExecutor;
    private readonly IPrintProgressExecutor _printProgressExecutor;
    private readonly IPipelineExecutor _pipelineExecutor;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IAfterPipelineLogger _afterPipelineLogger;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly ILogger<ExecutionOrchestrator> _logger;
    private readonly IExceptionBuffer _exceptionBuffer;

    private readonly object _lock = new();

    private bool _hasRun;

    public ExecutionOrchestrator(IPipelineInitializer pipelineInitializer,
        IModuleDisposeExecutor moduleDisposeExecutor,
        IPrintModuleOutputExecutor printModuleOutputExecutor,
        IPrintProgressExecutor printProgressExecutor,
        IPipelineExecutor pipelineExecutor,
        IConsolePrinter consolePrinter,
        IAfterPipelineLogger afterPipelineLogger,
        EngineCancellationToken engineCancellationToken,
        ILogger<ExecutionOrchestrator> logger,
        IExceptionBuffer exceptionBuffer)
    {
        _pipelineInitializer = pipelineInitializer;
        _moduleDisposeExecutor = moduleDisposeExecutor;
        _printModuleOutputExecutor = printModuleOutputExecutor;
        _printProgressExecutor = printProgressExecutor;
        _pipelineExecutor = pipelineExecutor;
        _consolePrinter = consolePrinter;
        _afterPipelineLogger = afterPipelineLogger;
        _engineCancellationToken = engineCancellationToken;
        _logger = logger;
        _exceptionBuffer = exceptionBuffer;
    }

    public async Task<PipelineSummary> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await ExecuteInternal(cancellationToken);
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
            _afterPipelineLogger.WriteLogs();
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

        var organizedModules = await _pipelineInitializer.Initialize();

        var runnableModules = organizedModules.RunnableModules.Select(x => x.Module).ToList();

        var start = DateTimeOffset.UtcNow;
        var stopWatch = Stopwatch.StartNew();

        try
        {
            var result = await ExecutePipeline(runnableModules, organizedModules);

            return await OnEnd(organizedModules, stopWatch, start, result);
        }
        catch
        {
            await OnEnd(organizedModules, stopWatch, start, null);
            throw;
        }
    }

    private async Task<PipelineSummary> OnEnd(OrganizedModules organizedModules, Stopwatch stopWatch, DateTimeOffset start,
        PipelineSummary? pipelineSummary)
    {
        var end = DateTimeOffset.UtcNow;
        pipelineSummary ??= new PipelineSummary(organizedModules.AllModules, stopWatch.Elapsed, start, end);

        _consolePrinter.PrintResults(pipelineSummary);

        await Console.Out.FlushAsync();

        // Flush any buffered exceptions after the results table has been printed
        _exceptionBuffer.FlushExceptions();

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
        await using var moduleDisposeExecutor = _moduleDisposeExecutor;
        using var printModuleOutputExecutor = _printModuleOutputExecutor;
        await using var printProgressExecutor = await _printProgressExecutor.InitializeAsync();

        return await _pipelineExecutor.ExecuteAsync(runnableModules, organizedModules);
    }
}