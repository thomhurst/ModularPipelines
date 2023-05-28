using Pipeline.NET.Modules;

namespace Pipeline.NET.Engine;

public interface IPipelineExecutor
{
    Task<IModule[]> ExecuteAsync();
}