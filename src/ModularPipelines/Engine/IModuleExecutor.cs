using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleExecutor
{
    Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules);

    Task<IEnumerable<IModule>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations) => ExecuteAsync(modules);
}
