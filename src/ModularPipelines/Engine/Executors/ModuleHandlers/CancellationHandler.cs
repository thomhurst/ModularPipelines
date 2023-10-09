using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class CancellationHandler : BaseHandler
{
    public CancellationHandler(ModuleBase module) : base(module)
    {
    }

    public void SetupCancellation()
    {
        if (Module.ModuleRunType != ModuleRunType.AlwaysRun)
        {
            EngineCancellationToken.Token.Register(Module.ModuleCancellationTokenSource.Cancel);
        }
        
        ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();
    }
}