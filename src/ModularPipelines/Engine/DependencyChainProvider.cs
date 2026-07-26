using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class DependencyChainProvider : IDependencyChainProvider
{
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; private set; } = [];

    public DependencyChainProvider(IModuleMetadataRegistry metadataRegistry)
    {
        _metadataRegistry = metadataRegistry;
    }

    public void Initialize(IReadOnlyList<IModule> modules)
    {
        // Finalize metadata for all modules before dependency resolution.
        // This ensures tags, categories, and custom attributes are merged from
        // all sources (attributes, instance overrides, registration-time configuration).
        foreach (var module in modules)
        {
            _metadataRegistry.FinalizeMetadata(module.GetType(), module);
        }

        ModuleDependencyModels = Detect(modules.Select(x => new ModuleDependencyModel(x)).ToArray());
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

        var dependencies = ModuleDependencyResolver.GetAllDependencies(
            moduleDependencyModel.Module,
            availableModuleTypes,
            dependencyContext: _metadataRegistry);

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
