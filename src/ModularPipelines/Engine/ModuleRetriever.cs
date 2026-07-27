using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleRetriever
{
    private readonly IModuleConditionHandler _moduleConditionHandler;
    private readonly IRegistrationEventExecutor _registrationEventExecutor;
    private readonly ISafeModuleEstimatedTimeProvider _estimatedTimeProvider;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly List<IModule> _modules;
    private Task<OrganizedModules>? _cached;

    public ModuleRetriever(
        IModuleConditionHandler moduleConditionHandler,
        IRegistrationEventExecutor registrationEventExecutor,
        IEnumerable<IModule> modules,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry
    )
    {
        _moduleConditionHandler = moduleConditionHandler;
        _registrationEventExecutor = registrationEventExecutor;
        _estimatedTimeProvider = estimatedTimeProvider;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;

        // Re-registering one pre-created module adds another service descriptor,
        // but the module object still represents one execution.
        _modules = modules
            .Distinct<IModule>(ReferenceEqualityComparer.Instance)
            .ToList();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Task<OrganizedModules> GetOrganizedModules(CancellationToken cancellationToken = default)
    {
        return _cached ??= GetInternal(cancellationToken);
    }

    internal async Task<IReadOnlyList<IModule>> GetRunnableModulesForValidation(
        CancellationToken cancellationToken = default)
    {
        return (await DiscoverModules(cancellationToken).ConfigureAwait(false)).RunnableModules;
    }

    private async Task<DiscoveredModules> DiscoverModules(CancellationToken cancellationToken)
    {
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }

        // Dynamic dependencies and metadata must be registered before conditions and cascade skipping are evaluated.
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(_modules).ConfigureAwait(false);

        var modulesToIgnore = new List<IgnoredModule>();
        var modulesToProcess = new List<IModule>();

        foreach (var module in _modules)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var (shouldIgnore, skipDecision) = await _moduleConditionHandler.ShouldIgnore(module, cancellationToken).ConfigureAwait(false);
            if (shouldIgnore)
            {
                modulesToIgnore.Add(new IgnoredModule(module, skipDecision ?? SkipDecision.Skip("Module was ignored")));
            }
            else
            {
                modulesToProcess.Add(module);
            }
        }

        CascadeSkippedDependencies(modulesToProcess, modulesToIgnore, cancellationToken);

        return new DiscoveredModules(modulesToProcess, modulesToIgnore);
    }

    private void CascadeSkippedDependencies(
        List<IModule> runnableModules,
        List<IgnoredModule> ignoredModules,
        CancellationToken cancellationToken)
    {
        var availableModuleTypes = _modules
            .Select(module => module.GetType())
            .Distinct()
            .ToArray();
        var runnableModuleTypes = runnableModules
            .Select(module => module.GetType())
            .ToHashSet();
        var ignoredModulesByType = ignoredModules
            .Where(ignoredModule => !runnableModuleTypes.Contains(ignoredModule.Module.GetType()))
            .GroupBy(ignoredModule => ignoredModule.Module.GetType())
            .ToDictionary(group => group.Key, group => group.First());
        var requiredDependenciesByModule = runnableModules.ToDictionary(
            module => module,
            module => ModuleDependencyResolver
                .GetAllDependencies(module, availableModuleTypes, _dependencyRegistry, _metadataRegistry)
                .Where(dependency => !dependency.Optional)
                .Select(dependency => dependency.DependencyType)
                .Distinct()
                .OrderBy(dependencyType => dependencyType.FullName, StringComparer.Ordinal)
                .ToArray());

        while (true)
        {
            var newlyIgnoredModules = new List<IgnoredModule>();

            foreach (var module in runnableModules)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var skippedDependencies = requiredDependenciesByModule[module]
                    .Where(ignoredModulesByType.ContainsKey)
                    .ToArray();

                if (skippedDependencies.Length == 0)
                {
                    continue;
                }

                newlyIgnoredModules.Add(new IgnoredModule(
                    module,
                    CreateDependencySkipDecision(skippedDependencies, ignoredModulesByType)));
            }

            if (newlyIgnoredModules.Count == 0)
            {
                return;
            }

            foreach (var ignoredModule in newlyIgnoredModules)
            {
                runnableModules.Remove(ignoredModule.Module);
                ignoredModules.Add(ignoredModule);
            }

            foreach (var ignoredModuleGroup in newlyIgnoredModules.GroupBy(
                         ignoredModule => ignoredModule.Module.GetType()))
            {
                var moduleType = ignoredModuleGroup.Key;
                if (runnableModules.All(module => module.GetType() != moduleType))
                {
                    ignoredModulesByType[moduleType] = ignoredModuleGroup.First();
                }
            }
        }
    }

    private static SkipDecision CreateDependencySkipDecision(
        IReadOnlyList<Type> skippedDependencies,
        IReadOnlyDictionary<Type, IgnoredModule> ignoredModulesByType)
    {
        if (skippedDependencies.Count == 1)
        {
            var dependencyType = skippedDependencies[0];
            var dependencyReason = ignoredModulesByType[dependencyType].SkipDecision.Reason;
            var reasonSuffix = string.IsNullOrWhiteSpace(dependencyReason)
                ? string.Empty
                : $": {dependencyReason}";
            return SkipDecision.Skip($"Required dependency '{dependencyType.Name}' was skipped{reasonSuffix}");
        }

        var dependencyNames = string.Join(
            ", ",
            skippedDependencies.Select(dependencyType => $"'{dependencyType.Name}'"));
        return SkipDecision.Skip($"Required dependencies {dependencyNames} were skipped");
    }

    private async Task<OrganizedModules> GetInternal(CancellationToken cancellationToken)
    {
        var discoveredModules = await DiscoverModules(cancellationToken).ConfigureAwait(false);
        var runnableModulesWithEstimatatedDuration = await discoveredModules.RunnableModules.ToAsyncProcessorBuilder()
            .SelectAsync(async module =>
            {
                var estimatedTime = await _estimatedTimeProvider.GetModuleEstimatedTimeAsync(module.GetType());

                var subModules = await _estimatedTimeProvider.GetSubModuleEstimatedTimesAsync(module.GetType());

                return new RunnableModule(module, estimatedTime, subModules.ToImmutableList());
            })
            .ProcessInParallel(100, TimeSpan.FromSeconds(1));

        return new OrganizedModules(
            RunnableModules: runnableModulesWithEstimatatedDuration,
            IgnoredModules: discoveredModules.IgnoredModules
        );
    }

    private sealed record DiscoveredModules(
        IReadOnlyList<IModule> RunnableModules,
        IReadOnlyList<IgnoredModule> IgnoredModules);
}
