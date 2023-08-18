using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class AsyncLocalModule : IAsyncLocalModule
{
    private readonly AsyncLocal<ModuleBase> _modules = new();
    
    public ModuleBase? GetActiveModule()
    {
        return _modules.Value;
    }

    public void SetActiveModule<T>(T moduleBase) where T : ModuleBase
    {
        _modules.Value = moduleBase;
    }
}