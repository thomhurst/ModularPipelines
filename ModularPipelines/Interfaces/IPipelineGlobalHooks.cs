using ModularPipelines.Context;

namespace ModularPipelines.Interfaces;

public interface IPipelineGlobalHooks
{
    Task OnStartAsync(IModuleContext moduleContext);
    Task OnEndAsync(IModuleContext moduleContext);
}