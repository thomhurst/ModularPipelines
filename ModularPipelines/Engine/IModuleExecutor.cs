using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleExecutor
{
    Task<IEnumerable<ModuleBase>> ExecuteAsync(IEnumerable<ModuleBase> modules);
}
