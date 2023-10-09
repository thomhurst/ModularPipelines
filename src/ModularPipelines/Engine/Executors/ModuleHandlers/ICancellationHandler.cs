namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface ICancellationHandler
{
    void SetupCancellation();
    
    Task ConfigureModuleTimeout();
}