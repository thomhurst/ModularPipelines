using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IModuleIgnoreHandler
{
    bool ShouldIgnore(ModuleBase module);
}
