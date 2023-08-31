using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class DependencyChainProvider : IDependencyChainProvider
{
    public IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; }

    public DependencyChainProvider(IEnumerable<ModuleBase> modules)
    {
        ModuleDependencyModels = Detect(modules.Select(x => new ModuleDependencyModel(x)).ToList());
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
        var customAttributes = moduleDependencyModel.Module.GetType().GetCustomAttributes<DependsOnAttribute>(true);

        foreach (var dependsOnAttribute in customAttributes)
        {
            var dependency = GetModuleDependencyModel(dependsOnAttribute.Type, allModules);
            
            if (dependency is not null)
            {
                yield return dependency;
            }
        }
    }

    private ModuleDependencyModel? GetModuleDependencyModel(Type type, IEnumerable<ModuleDependencyModel> allModules)
    {
        return allModules.FirstOrDefault(x => x.Module.GetType() == type);
    }
}
