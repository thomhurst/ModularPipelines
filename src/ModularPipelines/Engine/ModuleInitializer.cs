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

    public ModuleBase Initialize(ModuleBase module)
    {
        return module.Initialize(_moduleContextProvider.GetModuleContext());
    }
}
