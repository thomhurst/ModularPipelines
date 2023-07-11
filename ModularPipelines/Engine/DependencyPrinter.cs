using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

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
        var alreadyPrinted = new HashSet<ModuleDependencyModel>();
        
        var stringBuilder = new StringBuilder();

        foreach (var moduleDependencyModel in _dependencyChainProvider.ModuleDependencyModels.OrderBy(m => m.AllDescendantDependencies().Count()))
        {
            if (alreadyPrinted.Contains(moduleDependencyModel))
            {
                continue;
            }
            
            stringBuilder.AppendLine();
            Print(stringBuilder, moduleDependencyModel, 1, alreadyPrinted);
        }
        
        alreadyPrinted.Clear();

        _logger.LogInformation("The following dependency chains have been detected:\r\n{Chain}", stringBuilder.ToString());
    }

    private void Print(StringBuilder stringBuilder, ModuleDependencyModel moduleDependencyModel, int dashCount, ISet<ModuleDependencyModel> alreadyPrinted)
    {
        alreadyPrinted.Add(moduleDependencyModel);
        
        stringBuilder.Append(new string('-', dashCount));
        stringBuilder.Append('>');
        stringBuilder.Append(' ');
        stringBuilder.AppendLine(moduleDependencyModel.Module.GetType().Name);

        foreach (var dependencyModel in moduleDependencyModel.IsDependencyFor)
        {
            Print(stringBuilder, dependencyModel, dashCount + 2, alreadyPrinted);
        }
    }
}
