using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Interfaces;

public interface IPipelineGlobalHooks
{
    Task OnStartAsync(IModuleContext moduleContext);
    Task OnEndAsync(IModuleContext moduleContext, IReadOnlyList<ModuleBase> modules);
}