using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class DependencyPrinter : IDependencyPrinter
{
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly ILogger<DependencyPrinter> _logger;
    private readonly IInternalCollapsableLogging _collapsableLogging;
    private readonly IOptions<PipelineOptions> _options;

    public DependencyPrinter(IDependencyChainProvider dependencyChainProvider,
        ILogger<DependencyPrinter> logger,
        IInternalCollapsableLogging collapsableLogging,
        IOptions<PipelineOptions> options)
    {
        _dependencyChainProvider = dependencyChainProvider;
        _logger = logger;
        _collapsableLogging = collapsableLogging;
        _options = options;
    }

    public void PrintDependencyChains()
    {
        if (!_options.Value.PrintDependencyChains)
        {
            return;
        }
        
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
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }
        
        _logger.LogInformation("\n");
        _collapsableLogging.StartConsoleLogGroupDirectToConsole("Dependency Chains");
        _logger.LogInformation("The following dependency chains have been detected:\r\n{Chain}", value);
        _collapsableLogging.EndConsoleLogGroupDirectToConsole("Dependency Chains");
        _logger.LogInformation("\n");
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