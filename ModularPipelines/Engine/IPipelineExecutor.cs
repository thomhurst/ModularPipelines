using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IPipelineExecutor
{
    Task<IReadOnlyList<ModuleBase>> ExecuteAsync();
}