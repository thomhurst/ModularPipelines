using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

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
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IModuleResultRepository _resultRepository;
    private readonly IPipelineContextProvider _pipelineContextProvider;

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
        IExceptionBuffer exceptionBuffer,
        IModuleResultRegistry resultRegistry,
        IModuleResultRepository resultRepository,
        IPipelineContextProvider pipelineContextProvider)
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
        _resultRegistry = resultRegistry;
        _resultRepository = resultRepository;
        _pipelineContextProvider = pipelineContextProvider;
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

        // Register skipped results for ignored modules (via Category/RunCondition)
        await RegisterIgnoredModuleResultsAsync(organizedModules.IgnoredModules);

        var runnableModules = organizedModules.RunnableModules.Select(x => (IModule)x.Module).ToList();

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
        pipelineSummary ??= new PipelineSummary(organizedModules.AllModules, stopWatch.Elapsed, start, end, _resultRegistry);

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

    /// <summary>
    /// Registers skipped results for modules that were ignored via Category or RunCondition.
    /// This ensures tests and other code can retrieve results for these modules.
    /// If a history repository is configured and has a cached result, it will be used.
    /// </summary>
    private async Task RegisterIgnoredModuleResultsAsync(IReadOnlyList<IgnoredModule> ignoredModules)
    {
        var pipelineContext = _pipelineContextProvider.GetModuleContext();

        foreach (var ignoredModule in ignoredModules)
        {
            var module = ignoredModule.Module;
            var moduleType = module.GetType();
            var resultType = module.ResultType;

            // For ignored modules, always check for historical data if a repository is configured
            // (This is different from skipped modules which require IHistoryAware)
            if (_resultRepository.GetType() != typeof(NoOpModuleResultRepository))
            {
                var historicalResult = await TryGetHistoricalResultAsync(module, moduleType, resultType, pipelineContext);
                if (historicalResult != null)
                {
                    // Update the status to UsedHistory since we're using a cached result
                    if (historicalResult is ModuleResult moduleResult)
                    {
                        moduleResult.ModuleStatus = Status.UsedHistory;
                    }

                    _logger.LogDebug("Using historical result for ignored module {ModuleName}",
                        MarkupFormatter.FormatModuleName(moduleType.Name));
                    _resultRegistry.RegisterResult(moduleType, historicalResult);
                    continue;
                }
            }

            _logger.LogDebug("Registering skipped result for ignored module {ModuleName}",
                MarkupFormatter.FormatModuleName(moduleType.Name));

            // Create execution context with Skipped status
            var contextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);
            var executionContext = (ModuleExecutionContext)Activator.CreateInstance(contextType, module, moduleType)!;
            executionContext.Status = Status.Skipped;
            executionContext.SkipResult = ignoredModule.SkipDecision;

            // Create ModuleResult<T> with the skipped status using the value constructor (T?, ModuleExecutionContext)
            var resultGenericType = typeof(ModuleResult<>).MakeGenericType(resultType);
            var constructor = resultGenericType.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { resultType, typeof(ModuleExecutionContext) },
                null);

            if (constructor == null)
            {
                _logger.LogWarning("Could not find constructor for ModuleResult<{ResultType}>", resultType.Name);
                continue;
            }

            var result = (IModuleResult)constructor.Invoke(new object?[] { null, executionContext })!;

            _resultRegistry.RegisterResult(moduleType, result);
        }
    }

    /// <summary>
    /// Attempts to get a historical result for a module using reflection to call the generic GetResultAsync method.
    /// </summary>
    private async Task<IModuleResult?> TryGetHistoricalResultAsync(
        IModule module,
        Type moduleType,
        Type resultType,
        IPipelineContext pipelineContext)
    {
        try
        {
            // Get the generic GetResultAsync<T> method
            var getResultAsyncMethod = typeof(IModuleResultRepository)
                .GetMethod(nameof(IModuleResultRepository.GetResultAsync))!
                .MakeGenericMethod(resultType);

            // Invoke the method: Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineHookContext pipelineContext)
            var task = (Task?)getResultAsyncMethod.Invoke(_resultRepository, new object[] { module, pipelineContext });

            if (task == null)
            {
                return null;
            }

            await task.ConfigureAwait(false);

            // Get the Result property from the completed Task<ModuleResult<T>?>
            var resultProperty = task.GetType().GetProperty("Result");
            var historicalResult = resultProperty?.GetValue(task) as IModuleResult;

            return historicalResult;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get historical result for module {ModuleName}", moduleType.Name);
            return null;
        }
    }
}