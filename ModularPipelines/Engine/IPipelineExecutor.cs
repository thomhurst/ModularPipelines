using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IPipelineExecutor
{
    Task<IModule[]> ExecuteAsync();
}