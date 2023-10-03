using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class DependencyPrinter : IDependencyPrinter
{
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly ILogger<DependencyPrinter> _logger;
    private readonly ICollapsableLogging _collapsableLogging;

    public DependencyPrinter(IDependencyChainProvider dependencyChainProvider, 
        ILogger<DependencyPrinter> logger,
        ICollapsableLogging collapsableLogging)
    {
        _dependencyChainProvider = dependencyChainProvider;
        _logger = logger;
        _collapsableLogging = collapsableLogging;
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

        _collapsableLogging.StartConsoleLogGroup("Dependency Chains");
        _logger.LogInformation("The following dependency chains have been detected:\r\n{Chain}", stringBuilder.ToString());
        _collapsableLogging.EndConsoleLogGroup("Dependency Chains");
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
