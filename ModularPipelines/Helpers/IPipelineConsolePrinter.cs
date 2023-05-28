using ModularPipelines.Modules;

namespace ModularPipelines.Helpers;

public interface IPipelineConsolePrinter
{
    void PrintProgress(List<IModule> modulesToProcess, List<IModule> modulesToIgnore);
}