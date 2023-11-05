using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleExecutor
{
    Task<IEnumerable<ModuleBase>> ExecuteAsync(IReadOnlyList<ModuleBase> modules);
    
    Task<ModuleBase> ExecuteAsync(ModuleBase module);
}
