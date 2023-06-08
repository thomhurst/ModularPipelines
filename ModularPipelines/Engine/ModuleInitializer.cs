using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleInitializer : IModuleInitializer
{
    private readonly IModuleContextCreator _moduleContextCreator;

    public ModuleInitializer(IModuleContextCreator moduleContextCreator)
    {
        _moduleContextCreator = moduleContextCreator;
    }
    
    public ModuleBase Initialize(ModuleBase module)
    {
        return module.Initialize(_moduleContextCreator.CreateContext(module.GetType()));
    }
}