using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleInitializer
{
    IModule Initialize(IModule module);
}
