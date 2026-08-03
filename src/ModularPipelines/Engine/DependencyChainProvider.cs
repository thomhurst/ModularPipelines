using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class DependencyChainProvider : IDependencyChainProvider
{
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IModuleDependencyRegistry _dependencyRegistry;

    public IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; private set; } = [];

    public DependencyChainProvider(
        IModuleMetadataRegistry metadataRegistry,
        IModuleDependencyRegistry dependencyRegistry)
    {
        _metadataRegistry = metadataRegistry;
        _dependencyRegistry = dependencyRegistry;
    }

    public void Initialize(IReadOnlyList<IModule> modules)
    {
        // Finalize metadata for all modules before dependency resolution.
        // This ensures tags, categories, and custom attributes are merged from
        // both supported sources (attributes and module configuration).
        foreach (var module in modules)
        {
            _metadataRegistry.FinalizeMetadata(module.GetType(), module);
        }

        ModuleDependencyModels = Detect(modules.Select(x => new ModuleDependencyModel(x)).ToArray());
    }

    private ModuleDependencyModel[] Detect(ModuleDependencyModel[] allModules)
    {
        var availableModuleTypes = new Type[allModules.Length];
        var moduleModelsByType = new Dictionary<Type, ModuleDependencyModel>(allModules.Length);

        for (var i = 0; i < allModules.Length; i++)
        {
            var moduleModel = allModules[i];
            var moduleType = moduleModel.Module.GetType();
            availableModuleTypes[i] = moduleType;
            moduleModelsByType.TryAdd(moduleType, moduleModel);
        }

        foreach (var moduleDependencyModel in allModules)
        {
            var dependencies = GetModuleDependencies(
                    moduleDependencyModel,
                    availableModuleTypes,
                    moduleModelsByType)
                .ToArray();

            moduleDependencyModel.IsDependentOn.AddRange(dependencies);

            foreach (var dependencyModel in dependencies)
            {
                dependencyModel.IsDependencyFor.Add(moduleDependencyModel);
            }
        }

        return allModules;
    }

    private IEnumerable<ModuleDependencyModel> GetModuleDependencies(
        ModuleDependencyModel moduleDependencyModel,
        IReadOnlyList<Type> availableModuleTypes,
        Dictionary<Type, ModuleDependencyModel> moduleModelsByType)
    {
        var dependencies = ModuleDependencyResolver.GetAllDependencies(
            moduleDependencyModel.Module,
            availableModuleTypes,
            _dependencyRegistry,
            dependencyContext: _metadataRegistry);

        foreach (var dependencyType in dependencies
                     .Select(dependency => dependency.DependencyType)
                     .Distinct())
        {
            if (moduleModelsByType.TryGetValue(dependencyType, out var dependencyModel))
            {
                yield return dependencyModel;
            }
        }
    }
}
