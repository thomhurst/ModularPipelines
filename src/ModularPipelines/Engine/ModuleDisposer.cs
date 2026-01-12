using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleDisposer
{
    public async Task DisposeAsync(ModuleState moduleState)
    {
        await Disposer.DisposeObjectAsync(moduleState.Module).ConfigureAwait(false);
    }

    public async Task DisposeAsync(IModule module)
    {
        await Disposer.DisposeObjectAsync(module).ConfigureAwait(false);
    }
}
