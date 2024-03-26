using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

internal class ConsolePrinter : IConsolePrinter
{
    private readonly IProgressPrinter _progressPrinter;
    private readonly ILogoPrinter _logoPrinter;

    public ConsolePrinter(IProgressPrinter progressPrinter,
        ILogoPrinter logoPrinter)
    {
        _progressPrinter = progressPrinter;
        _logoPrinter = logoPrinter;
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
}