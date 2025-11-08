using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleDisposer : IModuleDisposer
{
    public async Task DisposeAsync(IModule module)
    {
        await Disposer.DisposeObjectAsync(module);

        // Only ModuleBase has Context property
        if (module is ModuleBase moduleBase)
        {
            await Disposer.DisposeObjectAsync(moduleBase.Context.Logger);
        }
    }
}