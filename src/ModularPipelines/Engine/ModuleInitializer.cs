using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IModuleContextProvider _moduleContextProvider;

    public ModuleInitializer(IModuleContextProvider moduleContextProvider)
    {
        _moduleContextProvider = moduleContextProvider;
    }

    public async Task<ModuleBase> Initialize(ModuleBase module)
    {
        return module.Initialize(await _moduleContextProvider.GetModuleContext());
    }
}
