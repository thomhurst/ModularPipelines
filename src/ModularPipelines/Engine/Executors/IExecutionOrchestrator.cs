using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors;

internal interface IExecutionOrchestrator
{
    Task<IReadOnlyList<ModuleBase>> ExecuteAsync();
}