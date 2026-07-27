using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Dependencies;

internal static class DependencySkipCascade
{
    public static async Task<DependencySkipCascadeResult> ApplyAsync(
        IReadOnlyCollection<IModule> allModules,
        IEnumerable<IModule> runnableModules,
        IEnumerable<IgnoredModule> ignoredModules,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        Func<IReadOnlyList<IgnoredModule>, Task> prepareIgnoredModules,
        Func<Type, bool> isSkipped,
        CancellationToken cancellationToken = default)
    {
        var availableModuleTypes = allModules
            .Select(module => module.GetType())
            .Distinct()
            .ToArray();
        var remainingModules = runnableModules.ToList();
        var remainingModuleSet = remainingModules.ToHashSet<IModule>(ReferenceEqualityComparer.Instance);
        var runnableModuleTypeCounts = remainingModules
            .GroupBy(module => module.GetType())
            .ToDictionary(group => group.Key, group => group.Count());
        var allIgnoredModules = ignoredModules.ToList();
        var pendingIgnoredModules = allIgnoredModules.ToList();
        var requiredDependenciesByModule = remainingModules.ToDictionary<IModule, IModule, Type[]>(
            module => module,
            module => ModuleDependencyResolver
                .GetAllDependencies(module, availableModuleTypes, dependencyRegistry, metadataRegistry)
                .Where(dependency => !dependency.Optional)
                .Select(dependency => dependency.DependencyType)
                .Distinct()
                .OrderBy(dependencyType => dependencyType.FullName, StringComparer.Ordinal)
                .ToArray(),
            ReferenceEqualityComparer.Instance);
        var dependentsByType = requiredDependenciesByModule
            .SelectMany(pair => pair.Value.Select(dependencyType => (DependencyType: dependencyType, Module: pair.Key)))
            .GroupBy(pair => pair.DependencyType)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(pair => pair.Module)
                    .Distinct<IModule>(ReferenceEqualityComparer.Instance)
                    .ToArray());
        var skippedIgnoredModulesByType = new Dictionary<Type, IgnoredModule>();

        while (pendingIgnoredModules.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await prepareIgnoredModules(pendingIgnoredModules).ConfigureAwait(false);

            var newlySkippedTypes = pendingIgnoredModules
                .Where(ignoredModule => !runnableModuleTypeCounts.ContainsKey(ignoredModule.Module.GetType()))
                .Where(ignoredModule => isSkipped(ignoredModule.Module.GetType()))
                .GroupBy(ignoredModule => ignoredModule.Module.GetType())
                .Where(group => !skippedIgnoredModulesByType.ContainsKey(group.Key))
                .Select(group =>
                {
                    skippedIgnoredModulesByType[group.Key] = group.First();
                    return group.Key;
                })
                .ToArray();
            var candidateModules = newlySkippedTypes
                .Where(dependentsByType.ContainsKey)
                .SelectMany(moduleType => dependentsByType[moduleType])
                .Where(remainingModuleSet.Contains)
                .Distinct<IModule>(ReferenceEqualityComparer.Instance);
            var newlyIgnoredModules = candidateModules
                .Select(module => new
                {
                    Module = module,
                    SkippedDependencies = requiredDependenciesByModule[module]
                        .Where(skippedIgnoredModulesByType.ContainsKey)
                        .ToArray(),
                })
                .Where(item => item.SkippedDependencies.Length > 0)
                .Select(item => new IgnoredModule(
                    item.Module,
                    DependencySkipDecisionFactory.Create(
                        item.SkippedDependencies
                            .Select(dependencyType => (
                                ModuleType: dependencyType,
                                SkipDecision: (SkipDecision?) skippedIgnoredModulesByType[dependencyType].SkipDecision))
                            .ToArray())))
                .ToList();

            if (newlyIgnoredModules.Count == 0)
            {
                break;
            }

            var newlyIgnoredModuleSet = newlyIgnoredModules
                .Select(ignoredModule => ignoredModule.Module)
                .ToHashSet<IModule>(ReferenceEqualityComparer.Instance);
            remainingModules.RemoveAll(newlyIgnoredModuleSet.Contains);
            remainingModuleSet.ExceptWith(newlyIgnoredModuleSet);

            foreach (var ignoredModule in newlyIgnoredModules)
            {
                var moduleType = ignoredModule.Module.GetType();
                if (--runnableModuleTypeCounts[moduleType] == 0)
                {
                    runnableModuleTypeCounts.Remove(moduleType);
                }
            }

            allIgnoredModules.AddRange(newlyIgnoredModules);
            pendingIgnoredModules = newlyIgnoredModules;
        }

        return new DependencySkipCascadeResult(remainingModules, allIgnoredModules);
    }
}

internal sealed record DependencySkipCascadeResult(
    IReadOnlyList<IModule> RunnableModules,
    IReadOnlyList<IgnoredModule> IgnoredModules);
