using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class DependencyDetector : IDependencyDetector
{
    private readonly IDependencyCollisionDetector _dependencyCollisionDetector;
    private readonly IDependencyPrinter _dependencyPrinter;

    public DependencyDetector(IDependencyCollisionDetector dependencyCollisionDetector,
        IDependencyPrinter dependencyPrinter)
    {
        _dependencyCollisionDetector = dependencyCollisionDetector;
        _dependencyPrinter = dependencyPrinter;
    }

    public void Check()
    {
        _dependencyCollisionDetector.CheckCollisions();
        _dependencyPrinter.Print();
    }
}

internal interface IDependencyPrinter
{
    void Print();
}

internal class DependencyPrinter : IDependencyPrinter
{
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly ILogger<DependencyPrinter> _logger;

    public DependencyPrinter(IDependencyChainProvider dependencyChainProvider, ILogger<DependencyPrinter> logger)
    {
        _dependencyChainProvider = dependencyChainProvider;
        _logger = logger;
    }
    
    public void Print()
    {
        var stringBuilder = new StringBuilder();

        foreach (var moduleDependencyModel in _dependencyChainProvider.ModuleDependencyModels)
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
}

internal interface IDependencyChainProvider
{
    IReadOnlyList<ModuleDependencyModel> ModuleDependencyModels { get; }
}

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
            yield return GetModuleDependencyModel(dependsOnAttribute.Type, allModules);
        }
    }

    private ModuleDependencyModel GetModuleDependencyModel(Type type, IEnumerable<ModuleDependencyModel> allModules)
    {
        return allModules.First(x => x.Module.GetType() == type);
    }
}