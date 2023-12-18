using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleDisposer
{
    Task DisposeAsync(ModuleBase module);
}