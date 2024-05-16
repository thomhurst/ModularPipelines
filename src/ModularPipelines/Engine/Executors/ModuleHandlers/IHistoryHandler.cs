using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface IHistoryHandler<T>
{
    Task<ModuleResult<T>?> SetupModuleFromHistory(string? skipDecisionReason);

    Task SaveResult(ModuleResult<T> moduleResult);
}