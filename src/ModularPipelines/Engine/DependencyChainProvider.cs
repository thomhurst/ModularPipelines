using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class DependencyChainProvider : IDependencyChainProvider, IInitializer
{
    private readonly IModuleRetriever _moduleRetriever;
    
    public IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; private set; } = null!;

    public DependencyChainProvider(IModuleRetriever moduleRetriever)
    {
        _moduleRetriever = moduleRetriever;
    }

    public int Order => int.MaxValue;

    public async Task InitializeAsync()
    {
        var modules = await _moduleRetriever.GetOrganizedModules();
        ModuleDependencyModels = Detect(modules.AllModules.Select(x => new ModuleDependencyModel(x)).ToList());
    }

    private List<ModuleDependencyModel> Detect(List<ModuleDependencyModel> allModules)
    {
        foreach (var moduleDependencyModel in allModules)
        {
            var dependencies = GetModuleDependencies(moduleDependencyModel, allModules).ToList();

            moduleDependencyModel.IsDependentOn.AddRange(dependencies);

            foreach (var dependencyModel in dependencies)
            {
                dependencyModel.IsDependencyFor.Add(moduleDependencyModel);
            }
        }

        return allModules;
    }

    private IEnumerable<ModuleDependencyModel> GetModuleDependencies(ModuleDependencyModel moduleDependencyModel, IReadOnlyCollection<ModuleDependencyModel> allModules)
    {
        var dependencies = moduleDependencyModel.Module.GetModuleDependencies();

        foreach (var dependency in dependencies)
        {
            var dependencyModel = GetModuleDependencyModel(dependency.DependencyType, allModules);

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