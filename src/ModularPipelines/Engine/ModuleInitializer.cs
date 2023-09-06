using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IPipelineContextProvider _moduleContextProvider;

    public ModuleInitializer(IPipelineContextProvider moduleContextProvider)
    {
        _moduleContextProvider = moduleContextProvider;
    }

    public ModuleBase Initialize(ModuleBase module)
    {
        return module.Initialize(_moduleContextProvider.GetModuleContext());
    }
}
