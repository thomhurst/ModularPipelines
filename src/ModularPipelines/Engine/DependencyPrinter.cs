using Microsoft.Extensions.Options;
using ModularPipelines.Interfaces;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Engine;

internal class DependencyPrinter : IDependencyPrinter
{
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly IConsoleWriter _consoleWriter;
    private readonly IBuildSystemFormatter _formatter;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IDependencyTreeFormatter _treeFormatter;

    public DependencyPrinter(IDependencyChainProvider dependencyChainProvider,
        IConsoleWriter consoleWriter,
        IBuildSystemFormatterProvider formatterProvider,
        IOptions<PipelineOptions> options,
        IDependencyTreeFormatter treeFormatter)
    {
        _dependencyChainProvider = dependencyChainProvider;
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

        var models = _dependencyChainProvider.ModuleDependencyModels;
        if (!models.Any())
        {
            return;
        }

        var tree = _treeFormatter.FormatTree(models);

        Print(tree);
    }

    private void Print(Tree tree)
    {
        var startCommand = _formatter.GetStartBlockCommand("Module Dependencies");
        if (startCommand != null)
        {
            _consoleWriter.LogToConsole(startCommand);
        }

        _consoleWriter.Write(tree);

        var endCommand = _formatter.GetEndBlockCommand("Module Dependencies");
        if (endCommand != null)
        {
            _consoleWriter.LogToConsole(endCommand);
        }
    }
}