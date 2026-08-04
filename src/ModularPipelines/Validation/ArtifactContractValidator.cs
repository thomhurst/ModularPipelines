using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Validation;

/// <summary>
/// Validates statically declared producer and consumer artifact contracts.
/// </summary>
internal sealed class ArtifactContractValidator : IPipelineValidator
{
    /// <inheritdoc />
    public int Order => 250;

    /// <inheritdoc />
    public ValidationResult Validate(IServiceProvider services)
    {
        var result = ValidateModules(services, services.GetServices<IModule>());
        if (!result.HasErrors)
        {
            return result;
        }

        var modules = GetRunnableModulesForArtifactValidationAsync(services)
            .GetAwaiter()
            .GetResult();
        return ValidateModules(services, modules.AvailableModules, modules.RunnableConsumerTypes);
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateAsync(IServiceProvider services)
    {
        var result = ValidateModules(services, services.GetServices<IModule>());
        if (!result.HasErrors)
        {
            return result;
        }

        var modules = await GetRunnableModulesForArtifactValidationAsync(services)
            .ConfigureAwait(false);
        return ValidateModules(services, modules.AvailableModules, modules.RunnableConsumerTypes);
    }

    private static async Task<ArtifactValidationModules> GetRunnableModulesForArtifactValidationAsync(
        IServiceProvider services)
    {
        var discoveredModules = await services.GetRequiredService<ModuleRetriever>()
            .GetUncascadedModulesForValidation()
            .ConfigureAwait(false);
        var runnableModules = discoveredModules.RunnableModules.ToList();
        var ignoredModules = discoveredModules.IgnoredModules.ToList();
        var modules = runnableModules
            .Concat(ignoredModules.Select(ignoredModule => ignoredModule.Module))
            .Distinct<IModule>(ReferenceEqualityComparer.Instance)
            .ToArray();
        var modulesByType = modules
            .GroupBy(module => module.GetType())
            .ToDictionary(group => group.Key, group => group.First());
        var dependencyRegistry = services.GetRequiredService<IModuleDependencyRegistry>();
        var metadataRegistry = services.GetRequiredService<IModuleMetadataRegistry>();
        var skipEvaluator = services.GetRequiredService<ModulePlanningSkipEvaluator>();

        var ignoredModuleTypes = ignoredModules
            .Select(ignoredModule => ignoredModule.Module.GetType())
            .ToHashSet();
        var moduleTypesWithHistory = new HashSet<Type>();
        var resultHistoryProvider = services.GetRequiredService<IModuleResultHistoryProvider>();
        var pipelineContext = services.GetRequiredService<IPipelineContextProvider>().GetModuleContext();
        foreach (var ignoredModule in ignoredModules)
        {
            if (await resultHistoryProvider
                    .TryGetAsync(ignoredModule.Module, pipelineContext)
                    .ConfigureAwait(false) is not null)
            {
                moduleTypesWithHistory.Add(ignoredModule.Module.GetType());
            }
        }

        var consumedArtifactProducerTypes = await GetConsumedArtifactProducerTypesAsync(
                runnableModules,
                modules,
                ignoredModuleTypes,
                moduleTypesWithHistory,
                dependencyRegistry,
                metadataRegistry,
                skipEvaluator)
            .ConfigureAwait(false);
        var cascadeResult = await DependencySkipCascade.ApplyAsync(
                modules,
                runnableModules,
                ignoredModules,
                dependencyRegistry,
                metadataRegistry,
                _ => Task.CompletedTask,
                moduleType => !moduleTypesWithHistory.Contains(moduleType)
                              || consumedArtifactProducerTypes.Contains(moduleType))
            .ConfigureAwait(false);
        var availableModules = cascadeResult.RunnableModules
            .Concat(cascadeResult.IgnoredModules
                .Select(ignoredModule => ignoredModule.Module)
                .Where(module => moduleTypesWithHistory.Contains(module.GetType())
                                 && !consumedArtifactProducerTypes.Contains(module.GetType())))
            .ToArray();
        return new ArtifactValidationModules(
            availableModules,
            cascadeResult.RunnableModules
                .Select(module => module.GetType())
                .ToHashSet());
    }

    private static async Task<HashSet<Type>> GetConsumedArtifactProducerTypesAsync(
        IReadOnlyCollection<IModule> runnableModules,
        IReadOnlyCollection<IModule> allModules,
        IReadOnlySet<Type> ignoredModuleTypes,
        IReadOnlySet<Type> moduleTypesWithHistory,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        ModulePlanningSkipEvaluator skipEvaluator)
    {
        var modulesByType = allModules
            .GroupBy(module => module.GetType())
            .ToDictionary(group => group.Key, group => group.First());
        var availableModuleTypes = modulesByType.Keys.ToArray();
        var moduleTypesWithoutHistory = ignoredModuleTypes
            .Where(moduleType => !moduleTypesWithHistory.Contains(moduleType))
            .ToHashSet();
        return await ArtifactDemandPlanner.ResolveAsync(async currentDemand =>
        {
            var nextConsumedArtifactProducerTypes = new HashSet<Type>();
            var unrecoverableModuleTypes = moduleTypesWithoutHistory
                .Concat(currentDemand)
                .ToHashSet();
            foreach (var module in runnableModules)
            {
                var consumedProducerTypes = module.GetType()
                    .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                    .Cast<ConsumesArtifactAttribute>()
                    .Where(IsValidArtifactDemand)
                    .Select(attribute => attribute.ProducerModule)
                    .Where(ignoredModuleTypes.Contains)
                    .ToHashSet();
                if (consumedProducerTypes.Count == 0
                    || HasUnrecoverableRequiredDependency(
                        module,
                        modulesByType,
                        availableModuleTypes,
                        ignoredModuleTypes,
                        unrecoverableModuleTypes,
                        consumedProducerTypes,
                        dependencyRegistry,
                        metadataRegistry))
                {
                    continue;
                }

                var skipDecision = await skipEvaluator
                    .EvaluateAsync(module, CancellationToken.None)
                    .ConfigureAwait(false);
                if (skipDecision?.ShouldSkip != true)
                {
                    nextConsumedArtifactProducerTypes.UnionWith(consumedProducerTypes);
                }
            }

            return nextConsumedArtifactProducerTypes;
        }).ConfigureAwait(false);
    }

    private static bool IsValidArtifactDemand(ConsumesArtifactAttribute consumedArtifact) =>
        consumedArtifact.ProducerModule
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .Count(producedArtifact => string.Equals(
                producedArtifact.Name,
                consumedArtifact.ArtifactName,
                StringComparison.Ordinal)) == 1;

    private static bool HasUnrecoverableRequiredDependency(
        IModule module,
        IReadOnlyDictionary<Type, IModule> modulesByType,
        IReadOnlyCollection<Type> availableModuleTypes,
        IReadOnlySet<Type> ignoredModuleTypes,
        IReadOnlySet<Type> unrecoverableModuleTypes,
        IReadOnlySet<Type> consumedProducerTypes,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry)
    {
        var pending = new Stack<IModule>();
        var visitedTypes = new HashSet<Type> { module.GetType() };
        pending.Push(module);
        while (pending.TryPop(out var currentModule))
        {
            var requiredDependencyTypes = ModuleDependencyResolver.GetAllDependencies(
                    currentModule,
                    availableModuleTypes,
                    dependencyRegistry,
                    metadataRegistry)
                .Where(dependency => !dependency.Optional)
                .Select(dependency => dependency.DependencyType);
            foreach (var dependencyType in requiredDependencyTypes)
            {
                if (ignoredModuleTypes.Contains(dependencyType))
                {
                    if (!consumedProducerTypes.Contains(dependencyType)
                        && unrecoverableModuleTypes.Contains(dependencyType))
                    {
                        return true;
                    }

                    continue;
                }

                if (visitedTypes.Add(dependencyType)
                    && modulesByType.TryGetValue(dependencyType, out var dependencyModule))
                {
                    pending.Push(dependencyModule);
                }
            }
        }

        return false;
    }

    private static ValidationResult ValidateModules(
        IServiceProvider services,
        IEnumerable<IModule> modules,
        IReadOnlySet<Type>? runnableConsumerTypes = null)
    {
        var result = new ValidationResult();
        var modulesByType = modules
            .GroupBy(module => module.GetType())
            .ToDictionary(group => group.Key, group => group.First());
        var moduleTypes = modulesByType.Keys.ToArray();
        var dependencyRegistry = services.GetRequiredService<IModuleDependencyRegistry>();
        var metadataRegistry = services.GetRequiredService<IModuleMetadataRegistry>();
        foreach (var (moduleType, module) in modulesByType)
        {
            metadataRegistry.FinalizeMetadata(moduleType, module);
        }

        foreach (var consumerType in moduleTypes.Where(
                     moduleType => runnableConsumerTypes?.Contains(moduleType) != false))
        {
            var consumedArtifacts = consumerType
                .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                .Cast<ConsumesArtifactAttribute>();

            foreach (var consumedArtifact in consumedArtifacts)
            {
                ValidateConsumedArtifact(
                    consumerType,
                    consumedArtifact,
                    modulesByType,
                    dependencyRegistry,
                    metadataRegistry,
                    result);
            }
        }

        return result;
    }

    private static void ValidateConsumedArtifact(
        Type consumerType,
        ConsumesArtifactAttribute consumedArtifact,
        IReadOnlyDictionary<Type, IModule> modulesByType,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        ValidationResult result)
    {
        var producerType = consumedArtifact.ProducerModule;
        if (!modulesByType.ContainsKey(producerType))
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Artifact,
                $"Module '{consumerType.Name}' consumes artifact '{consumedArtifact.ArtifactName}' " +
                $"from unregistered producer module '{producerType.Name}'.",
                consumerType));
            return;
        }

        var producedArtifacts = producerType
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .ToArray();

        var matchingArtifacts = producedArtifacts
            .Where(producedArtifact => string.Equals(
                producedArtifact.Name,
                consumedArtifact.ArtifactName,
                StringComparison.Ordinal))
            .ToArray();
        if (matchingArtifacts.Length == 0)
        {
            var availableArtifacts = producedArtifacts.Length == 0
                ? "none"
                : string.Join(", ", producedArtifacts.Select(artifact => $"'{artifact.Name}'"));
            result.AddError(new ValidationError(
                ValidationErrorCategory.Artifact,
                $"Module '{consumerType.Name}' consumes artifact '{consumedArtifact.ArtifactName}', " +
                $"but producer module '{producerType.Name}' does not declare it. " +
                $"Available artifacts: {availableArtifacts}.",
                consumerType));
            return;
        }

        if (matchingArtifacts.Length > 1)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Artifact,
                $"Module '{consumerType.Name}' consumes artifact '{consumedArtifact.ArtifactName}', " +
                $"but producer module '{producerType.Name}' declares that artifact name more than once.",
                consumerType));
            return;
        }

        if (!HasDependencyPath(
                consumerType,
                producerType,
                modulesByType,
                dependencyRegistry,
                metadataRegistry))
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Artifact,
                $"Module '{consumerType.Name}' consumes artifact '{consumedArtifact.ArtifactName}' " +
                $"from '{producerType.Name}' but does not depend on that producer through " +
                "required dependencies.",
                consumerType));
        }
    }

    private static bool HasDependencyPath(
        Type consumerType,
        Type producerType,
        IReadOnlyDictionary<Type, IModule> modulesByType,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry)
    {
        var availableModuleTypes = modulesByType.Keys.ToArray();
        var pending = new Queue<Type>();
        var visited = new HashSet<Type> { consumerType };
        pending.Enqueue(consumerType);

        while (pending.TryDequeue(out var moduleType))
        {
            if (!modulesByType.TryGetValue(moduleType, out var module))
            {
                continue;
            }

            foreach (var (dependencyType, optional) in ModuleDependencyResolver.GetAllDependencies(
                         module,
                         availableModuleTypes,
                         dependencyRegistry,
                         metadataRegistry))
            {
                if (optional)
                {
                    continue;
                }

                if (dependencyType == producerType)
                {
                    return true;
                }

                if (visited.Add(dependencyType))
                {
                    pending.Enqueue(dependencyType);
                }
            }
        }

        return false;
    }

    private sealed record ArtifactValidationModules(
        IReadOnlyList<IModule> AvailableModules,
        IReadOnlySet<Type> RunnableConsumerTypes);
}
