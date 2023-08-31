using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

internal class ConsolePrinter : IConsolePrinter
{
    private readonly IProgressPrinter _progressPrinter;
    private readonly ILogoPrinter _logoPrinter;
    private readonly IDependencyPrinter _dependencyPrinter;

    public ConsolePrinter(IProgressPrinter progressPrinter, 
        ILogoPrinter logoPrinter, 
        IDependencyPrinter dependencyPrinter)
    {
        _progressPrinter = progressPrinter;
        _logoPrinter = logoPrinter;
        _dependencyPrinter = dependencyPrinter;
    }

    public Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken)
    {
        return _progressPrinter.PrintProgress(organizedModules, cancellationToken);
    }

    public void PrintResults(PipelineSummary pipelineSummary)
    {
        _progressPrinter.PrintResults(pipelineSummary);
    }

    public void PrintLogo()
    {
        _logoPrinter.PrintLogo();
    }

    public void Print()
    {
        _dependencyPrinter.Print();
    }
}