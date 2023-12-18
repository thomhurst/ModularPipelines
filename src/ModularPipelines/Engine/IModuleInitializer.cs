using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleInitializer
{
    Task<ModuleBase> Initialize(ModuleBase module);
}