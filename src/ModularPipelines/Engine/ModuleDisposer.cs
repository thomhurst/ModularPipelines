using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleDisposer : IModuleDisposer
{
    public async Task DisposeAsync(ModuleBase module)
    {
        await Disposer.DisposeObjectAsync(module);
        await Disposer.DisposeObjectAsync(module.Context.Logger);
        
        var moduleLock = module.Lock;
        
        _ = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            await Disposer.DisposeObjectAsync(moduleLock);
        });

        module.Lock = null;

        if (!TestDetector.IsRunningFromNUnit)
        {
            await Disposer.DisposeObjectAsync(module.Context.ServiceProvider);
        }
    }
}