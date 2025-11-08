using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IPipelineContextProvider _moduleContextProvider;

    public ModuleInitializer(IPipelineContextProvider moduleContextProvider)
    {
        _moduleContextProvider = moduleContextProvider;
    }

    public IModule Initialize(IModule module)
    {
        // ModuleBase requires initialization to set Context
        if (module is ModuleBase moduleBase)
        {
            return moduleBase.Initialize(_moduleContextProvider.GetModuleContext());
        }

        // ModuleNew<T> doesn't require initialization - it uses IPipelineContext from ExecuteAsync parameter
        return module;
    }
}