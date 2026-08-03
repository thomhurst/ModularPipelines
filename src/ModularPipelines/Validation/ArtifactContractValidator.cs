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

        var runnableModules = GetRunnableModulesForArtifactValidationAsync(services)
            .GetAwaiter()
            .GetResult();
        return ValidateModules(services, runnableModules);
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateAsync(IServiceProvider services)
    {
        var result = ValidateModules(services, services.GetServices<IModule>());
        if (!result.HasErrors)
        {
            return result;
        }

        var runnableModules = await GetRunnableModulesForArtifactValidationAsync(services)
            .ConfigureAwait(false);
        return ValidateModules(services, runnableModules);
    }

    private static async Task<IReadOnlyList<IModule>> GetRunnableModulesForArtifactValidationAsync(
        IServiceProvider services)
    {
        var modules = await services.GetRequiredService<ModuleRetriever>()
            .GetRunnableModulesForValidation()
            .ConfigureAwait(false);
        var skipEvaluator = services.GetRequiredService<ModulePlanningSkipEvaluator>();
        var runnableModules = new List<IModule>(modules.Count);
        var ignoredModules = new List<IgnoredModule>();
        foreach (var module in modules)
        {
            var skipDecision = await skipEvaluator
                .EvaluateAsync(module, CancellationToken.None)
                .ConfigureAwait(false);
            if (skipDecision?.ShouldSkip == true)
            {
                ignoredModules.Add(new IgnoredModule(module, skipDecision));
            }
            else
            {
                runnableModules.Add(module);
            }
        }

        var consumedArtifactProducerTypes = runnableModules
            .SelectMany(module => module.GetType()
                .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                .Cast<ConsumesArtifactAttribute>())
            .Select(attribute => attribute.ProducerModule)
            .ToHashSet();
        var moduleTypesUsingHistory = new HashSet<Type>();
        var resultHistoryProvider = services.GetRequiredService<IModuleResultHistoryProvider>();
        var pipelineContext = services.GetRequiredService<IPipelineContextProvider>().GetModuleContext();
        var cascadeResult = await DependencySkipCascade.ApplyAsync(
                modules,
                runnableModules,
                ignoredModules,
                services.GetRequiredService<IModuleDependencyRegistry>(),
                services.GetRequiredService<IModuleMetadataRegistry>(),
                async pendingIgnoredModules =>
                {
                    foreach (var ignoredModule in pendingIgnoredModules)
                    {
                        if (!consumedArtifactProducerTypes.Contains(ignoredModule.Module.GetType())
                            && await resultHistoryProvider
                                .TryGetAsync(ignoredModule.Module, pipelineContext)
                                .ConfigureAwait(false) is not null)
                        {
                            moduleTypesUsingHistory.Add(ignoredModule.Module.GetType());
                        }
                    }
                },
                moduleType => !moduleTypesUsingHistory.Contains(moduleType))
            .ConfigureAwait(false);
        return cascadeResult.RunnableModules;
    }

    private static ValidationResult ValidateModules(
        IServiceProvider services,
        IEnumerable<IModule> modules)
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

        foreach (var consumerType in moduleTypes)
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
}
