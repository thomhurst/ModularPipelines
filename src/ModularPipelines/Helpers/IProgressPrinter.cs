using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

internal interface IProgressPrinter
{
    Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken);

    void PrintResults(PipelineSummary pipelineSummary);
}