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

        return Task.Run(async () =>
        {
            while (!Module.ResultTaskInternal.IsCompleted)
            {
                ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();
            
                if (Module.ModuleRunType != ModuleRunType.AlwaysRun)
                {
                    EngineCancellationToken.Token.ThrowIfCancellationRequested();
                }
                
                await Task.Delay(500, ModuleCancellationTokenSource.Token);
            }
        }, ModuleCancellationTokenSource.Token);
    }
}