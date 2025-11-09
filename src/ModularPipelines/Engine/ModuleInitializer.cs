using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    public IModule Initialize(IModule module)
    {
        // Module<T> doesn't require initialization - it uses IPipelineContext from ExecuteAsync parameter
        return module;
    }
}
