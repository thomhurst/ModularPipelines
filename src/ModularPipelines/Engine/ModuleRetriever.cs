using System.Collections.Immutable;
using System.Runtime.CompilerServices;
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
    private readonly List<IModule> _modules;
    private Task<OrganizedModules>? _cached;

    public ModuleRetriever(
        IModuleConditionHandler moduleConditionHandler,
        IModuleInitializer moduleInitializer,
        IEnumerable<IModule> modules,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider
    )
    {
        _moduleConditionHandler = moduleConditionHandler;
        _moduleInitializer = moduleInitializer;
        _estimatedTimeProvider = estimatedTimeProvider;
        _modules = modules.ToList();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Task<OrganizedModules> GetOrganizedModules()
    {
        return _cached ??= GetInternal();
    }

    private async Task<OrganizedModules> GetInternal()
    {
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }

        foreach (var module in _modules)
        {
            _moduleInitializer.Initialize(module);
        }

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
