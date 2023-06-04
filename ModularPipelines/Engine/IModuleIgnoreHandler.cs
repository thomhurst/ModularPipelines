using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleIgnoreHandler
{
    bool ShouldIgnore(ModuleBase module);
}