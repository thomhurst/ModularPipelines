using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleDisposer : IModuleDisposer
{
    public async Task DisposeAsync(ModuleBase module)
    {
        await Disposer.DisposeObjectAsync(module);
        
        await Disposer.DisposeObjectAsync(module.Context.Logger);

        if (!TestDetector.IsRunningFromNUnit)
        {
            await Disposer.DisposeObjectAsync(module.Context.ServiceProvider);
        }
    }
}