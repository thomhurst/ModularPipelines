using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Interfaces;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class DependencyPrinter : IDependencyPrinter
{
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly ILogger<DependencyPrinter> _logger;
    private readonly IConsoleWriter _consoleWriter;
    private readonly IBuildSystemFormatter _formatter;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IDependencyTreeFormatter _treeFormatter;

    public DependencyPrinter(IDependencyChainProvider dependencyChainProvider,
        ILogger<DependencyPrinter> logger,
        IConsoleWriter consoleWriter,
        IBuildSystemFormatterProvider formatterProvider,
        IOptions<PipelineOptions> options,
        IDependencyTreeFormatter treeFormatter)
    {
        _dependencyChainProvider = dependencyChainProvider;
        _logger = logger;
        _consoleWriter = consoleWriter;
        _formatter = formatterProvider.GetFormatter();
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

        var startCommand = _formatter.GetStartBlockCommand("Dependency Chains");
        if (startCommand != null)
        {
            _consoleWriter.LogToConsole(startCommand);
        }

        _logger.LogDebug("The following dependency chains have been detected:\r\n{Chain}", value);

        var endCommand = _formatter.GetEndBlockCommand("Dependency Chains");
        if (endCommand != null)
        {
            _consoleWriter.LogToConsole(endCommand);
        }

        _logger.LogDebug("\n");
    }
}