using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleDisposer : IModuleDisposer
{
    public async Task DisposeAsync(ModuleBase module)
    {
        await Disposer.DisposeAsync(module);
        
        await Disposer.DisposeAsync(module.Context.Logger);

        if (!TestDetector.IsRunningFromNUnit)
        {
            await Disposer.DisposeAsync(module.Context.ServiceProvider);
        }
    }
}