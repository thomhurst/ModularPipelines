using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal interface IAsyncLocalModule
{
    ModuleBase? GetActiveModule();
    void SetActiveModule<T>(T moduleBase) where T : ModuleBase;
}