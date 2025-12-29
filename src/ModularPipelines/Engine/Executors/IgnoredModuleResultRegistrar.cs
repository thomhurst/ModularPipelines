using System.Reflection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
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
                var historicalResult = await TryGetHistoricalResultAsync(module, moduleType, resultType, pipelineContext).ConfigureAwait(false);
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
            var executionContext = (ModuleExecutionContext) Activator.CreateInstance(contextType, module, moduleType)!;
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

            var result = (IModuleResult) constructor.Invoke(new object?[] { null, executionContext })!;

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
            var task = (Task?) getResultAsyncMethod.Invoke(_resultRepository, new object[] { module, pipelineContext });

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
