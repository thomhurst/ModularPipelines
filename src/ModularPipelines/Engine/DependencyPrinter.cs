using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal class DependencyPrinter : IDependencyPrinter
{
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly ILogger<DependencyPrinter> _logger;
    private readonly IInternalCollapsableLogging _collapsableLogging;

    public DependencyPrinter(IDependencyChainProvider dependencyChainProvider, 
        ILogger<DependencyPrinter> logger,
        IInternalCollapsableLogging collapsableLogging)
    {
        _dependencyChainProvider = dependencyChainProvider;
        _logger = logger;
        _collapsableLogging = collapsableLogging;
    }

    public void PrintDependencyChains()
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
            Append(stringBuilder, moduleDependencyModel, 1, alreadyPrinted);
        }

        alreadyPrinted.Clear();

        Print(stringBuilder.ToString());
    }

    private void Print(string value)
    {
        Console.WriteLine();
        _collapsableLogging.StartConsoleLogGroupInternal("Dependency Chains");
        _logger.LogInformation("The following dependency chains have been detected:\r\n{Chain}", value);
        _collapsableLogging.EndConsoleLogGroupInternal("Dependency Chains");
        Console.WriteLine();
    }

    private void Append(StringBuilder stringBuilder, ModuleDependencyModel moduleDependencyModel, int dashCount, ISet<ModuleDependencyModel> alreadyPrinted)
    {
        alreadyPrinted.Add(moduleDependencyModel);

        stringBuilder.Append(new string('-', dashCount));
        stringBuilder.Append('>');
        stringBuilder.Append(' ');
        stringBuilder.AppendLine(moduleDependencyModel.Module.GetType().Name);

        foreach (var dependencyModel in moduleDependencyModel.IsDependencyFor)
        {
            Append(stringBuilder, dependencyModel, dashCount + 2, alreadyPrinted);
        }
    }
}
