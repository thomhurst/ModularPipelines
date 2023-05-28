using Pipeline.NET.Modules;

namespace Pipeline.NET.Helpers;

public interface IPipelineConsolePrinter
{
    void PrintProgress(List<IModule> modulesToProcess, List<IModule> modulesToIgnore);
}