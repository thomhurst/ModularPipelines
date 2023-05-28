using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

public interface IModuleIgnoreHandler
{
    bool ShouldIgnore(IModule module);
}