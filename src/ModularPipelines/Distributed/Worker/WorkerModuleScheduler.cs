using System.Threading.Channels;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Worker;

/// <summary>
/// Minimal <see cref="IModuleScheduler"/> implementation for the worker execution path.
/// The distributed coordinator handles scheduling; this satisfies the
/// <see cref="ModularPipelines.Engine.Execution.IModuleRunner"/> contract.
/// </summary>
internal sealed class WorkerModuleScheduler : IModuleScheduler
{
    private static readonly Channel<ModuleState> EmptyChannel = Channel.CreateUnbounded<ModuleState>();

    public ChannelReader<ModuleState> ReadyModules => EmptyChannel.Reader;

    public void InitializeModules(IEnumerable<IModule> modules)
    {
    }

    public Task RunSchedulerAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public bool MarkModuleStarted(Type moduleType) => true;

    public void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null, ModuleStatus? statusOverride = null)
    {
    }

    public Task<IModule>? GetModuleCompletionTask(Type moduleType) => null;

    public ModuleState? GetModuleState(Type moduleType) => null;

    public IReadOnlyList<IModule> CancelPendingModules()
    {
        return [];
    }

    public void Dispose()
    {
    }
}
