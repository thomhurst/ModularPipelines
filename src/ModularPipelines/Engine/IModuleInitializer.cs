using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleInitializer
{
    ModuleBase Initialize(ModuleBase module);
}
