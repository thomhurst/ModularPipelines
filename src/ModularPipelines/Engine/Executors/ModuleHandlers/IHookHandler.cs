using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface IHookHandler<T>
{
    Task OnBeforeExecute(IPipelineContext context);

    Task OnAfterExecute(IPipelineContext context, ModuleResult<T> moduleResult);
}