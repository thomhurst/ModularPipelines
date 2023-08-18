using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

internal interface IPipelineConsolePrinter
{
    Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken);
}
