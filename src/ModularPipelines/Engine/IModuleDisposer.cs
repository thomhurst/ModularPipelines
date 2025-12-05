using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleDisposer
{
    Task DisposeAsync(ModuleState moduleState);

    Task DisposeAsync(IModule module);
}