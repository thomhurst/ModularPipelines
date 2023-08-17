using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleStatusProvider : IModuleStatusProvider
{
    private readonly IEnumerable<ModuleBase> _modules;

    public ModuleStatusProvider(IEnumerable<ModuleBase> modules)
    {
        _modules = modules;
    }
    
    public Status? GetStatusForModule<T>()
    {
        var module = _modules.OfType<T>().FirstOrDefault();
        
        return (module as ModuleBase)?.Status;
    }
}