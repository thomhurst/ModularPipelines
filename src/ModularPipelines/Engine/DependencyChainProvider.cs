using Initialization.Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class DependencyChainProvider : IDependencyChainProvider, IInitializer
{
    private readonly ModuleRetriever _moduleRetriever;
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; private set; } = [];

    public DependencyChainProvider(
        ModuleRetriever moduleRetriever,
        IModuleMetadataRegistry metadataRegistry)
    {
        _moduleRetriever = moduleRetriever;
        _metadataRegistry = metadataRegistry;
    }

    public int Order => int.MaxValue;

    public async Task InitializeAsync()
    {
        var modules = await _moduleRetriever.GetOrganizedModules().ConfigureAwait(false);

        // Finalize metadata for all modules before dependency resolution.
        // This ensures tags, categories, and custom attributes are merged from
        // all sources (attributes, instance overrides, registration-time configuration).
        foreach (var module in modules.AllModules)
        {
            _metadataRegistry.FinalizeMetadata(module.GetType(), module);
        }

        ModuleDependencyModels = Detect(modules.AllModules.Select(x => new ModuleDependencyModel(x)).ToArray());
    }

    private ModuleDependencyModel[] Detect(ModuleDependencyModel[] allModules)
    {
        foreach (var moduleDependencyModel in allModules)
        {
            var dependencies = GetModuleDependencies(moduleDependencyModel, allModules).ToArray();

            moduleDependencyModel.IsDependentOn.AddRange(dependencies);

            foreach (var dependencyModel in dependencies)
            {
                dependencyModel.IsDependencyFor.Add(moduleDependencyModel);
            }
        }

        return allModules;
    }

    private IEnumerable<ModuleDependencyModel> GetModuleDependencies(ModuleDependencyModel moduleDependencyModel, ModuleDependencyModel[] allModules)
    {
        // Get all available module types for DependsOnAllModulesInheritingFrom and predicate-based resolution
        var availableModuleTypes = allModules.Select(m => m.Module.GetType()).ToArray();

        // Pass the metadata registry as IDependencyContext for predicate-based dependencies
        var dependencies = ModuleDependencyResolver.GetDependencies(
            moduleDependencyModel.Module.GetType(),
            availableModuleTypes,
            _metadataRegistry);

        foreach (var (dependencyType, _) in dependencies)
        {
            var dependencyModel = GetModuleDependencyModel(dependencyType, allModules);

            if (dependencyModel is not null)
            {
                yield return dependencyModel;
            }
        }
    }

    private ModuleDependencyModel? GetModuleDependencyModel(Type type, IEnumerable<ModuleDependencyModel> allModules)
    {
        return allModules.FirstOrDefault(x => x.Module.GetType() == type);
    }
}