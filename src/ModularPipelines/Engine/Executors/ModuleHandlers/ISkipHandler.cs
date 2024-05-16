using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface ISkipHandler<T>
{
    Task<ModuleResult<T>?> HandleSkipped();
}