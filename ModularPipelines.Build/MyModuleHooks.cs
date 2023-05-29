using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;

namespace ModularPipelines.Build
{
    public class MyModuleHooks : IPipelineModuleHooks
    {
        public Task OnBeforeModuleStartAsync(IModuleContext moduleContext, IModule module)
        {
            moduleContext.Logger.LogInformation("{Module} is starting", module.GetType().Name);
            return Task.CompletedTask;
        }

        public Task OnBeforeModuleEndAsync(IModuleContext moduleContext, IModule module)
        {
            moduleContext.Logger.LogInformation("{Module} finished after {Elapsed}", module.GetType().Name, module.Duration);
            return Task.CompletedTask;
        }
    }
}