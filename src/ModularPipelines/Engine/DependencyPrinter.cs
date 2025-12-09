using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Interfaces;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class DependencyPrinter : IDependencyPrinter
{
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly ILogger<DependencyPrinter> _logger;
    private readonly IInternalCollapsableLogging _collapsableLogging;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IDependencyTreeFormatter _treeFormatter;

    public DependencyPrinter(IDependencyChainProvider dependencyChainProvider,
        ILogger<DependencyPrinter> logger,
        IInternalCollapsableLogging collapsableLogging,
        IOptions<PipelineOptions> options,
        IDependencyTreeFormatter treeFormatter)
    {
        _dependencyChainProvider = dependencyChainProvider;
        _logger = logger;
        _collapsableLogging = collapsableLogging;
        _options = options;
        _treeFormatter = treeFormatter;
    }

    public void PrintDependencyChains()
    {
        if (!_options.Value.PrintDependencyChains)
        {
            return;
        }

        var formattedTree = _treeFormatter.FormatTree(_dependencyChainProvider.ModuleDependencyModels);

        Print(formattedTree);
    }

    private void Print(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        _logger.LogDebug("\n");
        _collapsableLogging.StartConsoleLogGroupDirectToConsole("Dependency Chains", LogLevel.Debug);
        _logger.LogDebug("The following dependency chains have been detected:\r\n{Chain}", value);
        _collapsableLogging.EndConsoleLogGroupDirectToConsole("Dependency Chains", LogLevel.Debug);
        _logger.LogDebug("\n");
    }
}