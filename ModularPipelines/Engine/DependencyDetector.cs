using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines;

public class DependencyDetector : IDependencyDetector
{
    private readonly ILogger<DependencyDetector> _logger;
    public IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; }

    public DependencyDetector(IEnumerable<ModuleBase> modules, ILogger<DependencyDetector> logger)
    {
        _logger = logger;
        ModuleDependencyModels = Detect(modules.Select(x => new ModuleDependencyModel(x)).ToList());
    }

    public void Print()
    {
        var stringBuilder = new StringBuilder();

        foreach (var moduleDependencyModel in ModuleDependencyModels)
        {
            stringBuilder.AppendLine();
            Print(stringBuilder, moduleDependencyModel, 1);
        }
        
        _logger.LogInformation("The following dependency chains have been detected:\r\n{Chain}", stringBuilder.ToString());
    }

    private void Print(StringBuilder stringBuilder, ModuleDependencyModel moduleDependencyModel, int i)
    {
        stringBuilder.Append(new string('-', i));
        stringBuilder.Append(' ');
        stringBuilder.AppendLine(moduleDependencyModel.Module.GetType().Name);

        foreach (var dependencyModel in moduleDependencyModel.IsDependencyFor)
        {
            Print(stringBuilder, dependencyModel, i+2);
        }
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

        return allModules.Where(x => !x.IsDependentOn.Any()).ToList();
    }

    private IEnumerable<ModuleDependencyModel> GetModuleDependencies(ModuleDependencyModel moduleDependencyModel, IReadOnlyCollection<ModuleDependencyModel> allModules)
    {
        var customAttributes = moduleDependencyModel.Module.GetType().GetCustomAttributes<DependsOnAttribute>(true);

        foreach (var dependsOnAttribute in customAttributes)
        {
            yield return GetModuleDependencyModel(dependsOnAttribute.Type, allModules);
        }
    }

    private ModuleDependencyModel GetModuleDependencyModel(Type type, IEnumerable<ModuleDependencyModel> allModules)
    {
        return allModules.First(x => x.Module.GetType() == type);
    }
}

public record ModuleDependencyModel(ModuleBase Module)
{
    public List<ModuleDependencyModel> IsDependencyFor { get; } = new();
    public List<ModuleDependencyModel> IsDependentOn { get; } = new();
}