using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface ISkipHandler
{
    Task CallbackTask { get; }

    Task SetSkipped(SkipDecision skipDecision);

    Task<bool> HandleSkipped();
}