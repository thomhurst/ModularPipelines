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

            // Create execution context with Skipped status using compiled delegate factory
            var executionContext = ExecutionContextFactory.Create(module, moduleType);
            executionContext.Status = Status.Skipped;
            executionContext.SkipResult = ignoredModule.SkipDecision;

            // Create ModuleResult<T> with the skipped status using compiled delegate factory
            var result = ModuleResultFactory.CreateSkipped(resultType, executionContext);

            _resultRegistry.RegisterResult(moduleType, result);
        }
    }

    /// <summary>
    /// Attempts to get a historical result for a module using compiled delegates to call the generic GetResultAsync method.
    /// </summary>
    private async Task<IModuleResult?> TryGetHistoricalResultAsync(
        IModule module,
        Type resultType,
        IPipelineContext pipelineContext)
    {
        try
        {
            // Use compiled delegate instead of MakeGenericMethod + Invoke + GetProperty("Result")
            var getResultDelegate = ResultRepositoryDelegateFactory.GetResultDelegateFor(resultType);
            return await getResultDelegate(_resultRepository, module, pipelineContext).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get historical result for module {ModuleName}", module.GetType().Name);
            return null;
        }
    }
}
