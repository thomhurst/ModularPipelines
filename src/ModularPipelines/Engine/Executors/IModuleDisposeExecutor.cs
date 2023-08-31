using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IModuleDisposeExecutor
{
    Task<IReadOnlyList<ModuleBase>> ExecuteAndDispose(IEnumerable<ModuleBase> modules, Func<Task<IReadOnlyList<ModuleBase>>> executeDelegate);
}