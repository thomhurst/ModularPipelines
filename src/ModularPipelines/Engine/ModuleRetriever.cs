using System.Collections.Immutable;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleRetriever : IModuleRetriever
{
    private readonly IModuleConditionHandler _moduleConditionHandler;
    private readonly IModuleInitializer _moduleInitializer;
    private readonly ISafeModuleEstimatedTimeProvider _estimatedTimeProvider;
    private readonly List<ModuleBase> _modules;

    public ModuleRetriever(
        IModuleConditionHandler moduleConditionHandler,
        IModuleInitializer moduleInitializer,
        IEnumerable<ModuleBase> modules,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider
    )
    {
        _moduleConditionHandler = moduleConditionHandler;
        _moduleInitializer = moduleInitializer;
        _estimatedTimeProvider = estimatedTimeProvider;
        _modules = modules.ToList();
    }

    public async Task<OrganizedModules> GetOrganizedModules()
    {
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }

        await _modules
            .ToAsyncProcessorBuilder()
            .ForEachAsync(m => _moduleInitializer.Initialize(m))
            .ProcessInParallel();

        var modulesToIgnore = await _modules
            .WhereAsync(async m => await _moduleConditionHandler.ShouldIgnore(m))
            .ToListAsync();

        var modulesToProcess = _modules
            .Except(modulesToIgnore)
            .ToList();

        var runnableModulesWithEstimatatedDuration = await modulesToProcess.ToAsyncProcessorBuilder()
            .SelectAsync(async module =>
            {
                var estimatedTime = await _estimatedTimeProvider.GetModuleEstimatedTimeAsync(module.GetType());

                var subModules = await _estimatedTimeProvider.GetSubModuleEstimatedTimesAsync(module.GetType());

                return new RunnableModule(module, estimatedTime, subModules.ToImmutableList());
            })
            .ProcessInParallel(100, TimeSpan.FromSeconds(1));

        return new OrganizedModules(
            RunnableModules: runnableModulesWithEstimatatedDuration,
            IgnoredModules: modulesToIgnore
        );
    }
}
