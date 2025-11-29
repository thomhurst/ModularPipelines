using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleDisposer : IModuleDisposer
{
    private readonly IModuleLoggerProvider _loggerProvider;

    public ModuleDisposer(IModuleLoggerProvider loggerProvider)
    {
        _loggerProvider = loggerProvider;
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

        var logger = _loggerProvider.GetLogger(moduleType);
        await Disposer.DisposeObjectAsync(logger);
    }
}