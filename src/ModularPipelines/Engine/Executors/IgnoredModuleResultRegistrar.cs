using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

/// <summary>
/// Registers skipped results for modules that were ignored via Category or RunCondition.
/// This ensures tests and other code can retrieve results for these modules.
/// If a history repository is configured and has a cached result, it will be used.
/// </summary>
internal class IgnoredModuleResultRegistrar : IIgnoredModuleResultRegistrar
{
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IModuleResultRepository _resultRepository;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly ILogger<IgnoredModuleResultRegistrar> _logger;

    public IgnoredModuleResultRegistrar(
        IModuleResultRegistry resultRegistry,
        IModuleResultRepository resultRepository,
        IPipelineContextProvider pipelineContextProvider,
        ILogger<IgnoredModuleResultRegistrar> logger)
    {
        _resultRegistry = resultRegistry;
        _resultRepository = resultRepository;
        _pipelineContextProvider = pipelineContextProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task RegisterIgnoredModuleResultsAsync(IReadOnlyList<IgnoredModule> ignoredModules)
    {
        var pipelineContext = _pipelineContextProvider.GetModuleContext();

        foreach (var ignoredModule in ignoredModules)
        {
            var module = ignoredModule.Module;
            var moduleType = module.GetType();
            var resultType = module.ResultType;

            // For ignored modules, always check for historical data if a repository is configured
            if (_resultRepository.GetType() != typeof(NoOpModuleResultRepository))
            {
                var historicalResult = await TryGetHistoricalResultAsync(module, resultType, pipelineContext).ConfigureAwait(false);
                if (historicalResult != null)
                {
                    // Update the status to UsedHistory using the factory method
                    var usedHistoryResult = ModuleResultFactory.WithStatus(historicalResult, Status.UsedHistory);
                    _logger.LogDebug("Using historical result for ignored module {ModuleName}",
                        moduleType.Name);
                    _resultRegistry.RegisterResult(moduleType, usedHistoryResult);

                    // Set the completion source so awaiting the module returns immediately
                    SetModuleCompletionSource(module, resultType, usedHistoryResult);
                    continue;
                }
            }

            _logger.LogDebug("Registering skipped result for ignored module {ModuleName}",
                moduleType.Name);

            // Create execution context with Skipped status using compiled delegate factory
            var executionContext = ExecutionContextFactory.Create(module, moduleType);
            executionContext.Status = Status.Skipped;
            executionContext.SkipResult = ignoredModule.SkipDecision;

            // Create ModuleResult<T> with the skipped status using compiled delegate factory
            var result = ModuleResultFactory.CreateSkipped(resultType, executionContext);

            _resultRegistry.RegisterResult(moduleType, result);

            // Set the completion source so awaiting the module returns immediately
            SetModuleCompletionSource(module, resultType, result);
        }
    }

    /// <summary>
    /// Sets the completion source on a module so that awaiting the module returns immediately.
    /// This is necessary for ignored modules so that dependent modules don't wait forever.
    /// </summary>
    private static void SetModuleCompletionSource(IModule module, Type resultType, IModuleResult result)
    {
        var setter = CompletionSourceSetterCache.GetOrCreate(resultType);
        setter(module, result);
    }

    /// <summary>
    /// Attempts to get a historical result for a module using compiled delegates to call the generic GetResultAsync method.
    /// </summary>
    private async Task<IModuleResult?> TryGetHistoricalResultAsync(
        IModule module,
        Type resultType,
        IPipelineHookContext pipelineContext)
    {
        try
        {
            // Use compiled delegate instead of MakeGenericMethod + Invoke + GetProperty("Result")
            var getResultDelegate = ResultRepositoryDelegateFactory.GetResultDelegateFor(resultType);
            return await getResultDelegate(_resultRepository, module, pipelineContext).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogWarning(ex, "Failed to get historical result for module {ModuleName}", module.GetType().Name);
            return null;
        }
    }
}

/// <summary>
/// Cache for compiled delegates that set the completion source on a module.
/// </summary>
internal static class CompletionSourceSetterCache
{
    private static readonly ConcurrentDictionary<Type, Action<IModule, IModuleResult>> Cache = new();

    /// <summary>
    /// Gets or creates a compiled delegate that sets the completion source on a module.
    /// </summary>
    public static Action<IModule, IModuleResult> GetOrCreate(Type resultType)
    {
        return Cache.GetOrAdd(resultType, CreateSetter);
    }

    private static Action<IModule, IModuleResult> CreateSetter(Type resultType)
    {
        // Create compiled delegate for: ((Module<T>)module).CompletionSource.TrySetResult((ModuleResult<T?>)result)
        var moduleType = typeof(Module<>).MakeGenericType(resultType);

        // Parameters
        var moduleParam = Expression.Parameter(typeof(IModule), "module");
        var resultParam = Expression.Parameter(typeof(IModuleResult), "result");

        // Cast module to Module<T>
        var castModule = Expression.Convert(moduleParam, moduleType);

        // Access CompletionSource property
        var completionSourceProp = moduleType.GetProperty("CompletionSource",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"CompletionSource property not found on {moduleType.Name}");
        var completionSource = Expression.Property(castModule, completionSourceProp);

        // Get the actual property type and its TrySetResult method
        // This ensures we use the correct generic type as declared on the property
        var completionSourceType = completionSourceProp.PropertyType;
        var moduleResultType = completionSourceType.GetGenericArguments()[0]; // ModuleResult<T?>

        // Cast result to ModuleResult<T?>
        var castResult = Expression.Convert(resultParam, moduleResultType);

        // Call TrySetResult using the method from the actual property type
        var trySetResultMethod = completionSourceType.GetMethod("TrySetResult")!;
        var callTrySetResult = Expression.Call(completionSource, trySetResultMethod, castResult);

        // Compile to Action<IModule, IModuleResult>
        var lambda = Expression.Lambda<Action<IModule, IModuleResult>>(callTrySetResult, moduleParam, resultParam);
        return lambda.Compile();
    }
}
