using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal class CancellationHandler<T> : BaseHandler<T>, ICancellationHandler
{
    public CancellationHandler(Module<T> module) : base(module)
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

    public Task ConfigureModuleTimeout()
    {
        if (Module.Timeout == TimeSpan.Zero)
        {
            return Task.CompletedTask;
        }

        ModuleCancellationTokenSource.CancelAfter(Module.Timeout);
        
        return Task.Delay(Module.Timeout + TimeSpan.FromSeconds(30), ModuleCancellationTokenSource.Token);
    }
}