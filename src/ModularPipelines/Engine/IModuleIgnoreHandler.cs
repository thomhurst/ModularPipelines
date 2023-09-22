using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleIgnoreHandler
{
    Task<bool> ShouldIgnore(ModuleBase module);
}
