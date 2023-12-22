using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface IHistoryHandler<T>
{
    Task<bool> SetupModuleFromHistory(string? skipDecisionReason);

    Task SaveResult(ModuleResult<T> moduleResult);
}