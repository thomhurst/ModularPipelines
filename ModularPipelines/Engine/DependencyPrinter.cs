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
            Print(stringBuilder, dependencyModel, i + 2);
        }
    }
}
