using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.Build;

public class MyModuleHooks : IPipelineModuleHooks
{
    /// <inheritdoc/>
    public Task OnBeforeModuleStartAsync(IPipelineHookContext pipelineContext, IModule module)
    {
        pipelineContext.Logger.LogInformation("{Module} is starting at {DateTime}", module.GetType().Name, DateTimeOffset.UtcNow);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task OnAfterModuleEndAsync(IPipelineHookContext pipelineContext, IModule module)
    {
        var duration = (module as ModuleBase)?.Duration ?? TimeSpan.Zero;
        pipelineContext.Logger.LogInformation("{Module} finished at {DateTime} after {Elapsed}", module.GetType().Name, DateTimeOffset.UtcNow, duration);
        return Task.CompletedTask;
    }
}