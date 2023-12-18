using ModularPipelines.Context;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface IHookHandler
{
    Task OnBeforeExecute(IPipelineContext context);

    Task OnAfterExecute(IPipelineContext context);
}