using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleDisposer : IModuleDisposer
{
    private readonly IModuleLoggerContainer _loggerContainer;

    public ModuleDisposer(IModuleLoggerContainer loggerContainer)
    {
        _loggerContainer = loggerContainer;
    }

    public async Task DisposeAsync(ModuleState moduleState)
    {
        await DisposeModuleCore(moduleState.Module, moduleState.ModuleType);
    }

    public async Task DisposeAsync(IModule module)
    {
        await DisposeModuleCore(module, module.GetType());
    }

    private async Task DisposeModuleCore(IModule module, Type moduleType)
    {
        await Disposer.DisposeObjectAsync(module);

        var logger = _loggerContainer.GetLogger(moduleType);
        await Disposer.DisposeObjectAsync(logger);
    }
}