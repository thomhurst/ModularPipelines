using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    public IModule Initialize(IModule module)
    {
        // No initialization needed for composition-based modules
        // The module context is passed directly to ExecuteAsync
        return module;
    }
}