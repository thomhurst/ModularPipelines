using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

internal interface IPipelineConsolePrinter
{
    void PrintProgress(OrganizedModules organizedModules);
}