using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IModuleContext _moduleContext;

    public ModuleInitializer(IModuleContext moduleContext)
    {
        _moduleContext = moduleContext;
    }
    
    public ModuleBase Initialize(ModuleBase module)
    {
        return module.Initialize(_moduleContext);
    }
}