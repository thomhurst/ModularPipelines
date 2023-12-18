using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IPipelineContextProvider _moduleContextProvider;

    public ModuleInitializer(IPipelineContextProvider moduleContextProvider)
    {
        _moduleContextProvider = moduleContextProvider;
    }

    public async Task<ModuleBase> Initialize(ModuleBase module)
    {
        return module.Initialize(await _moduleContextProvider.GetModuleContext());
    }
}