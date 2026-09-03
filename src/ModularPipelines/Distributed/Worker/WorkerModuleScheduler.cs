using System.Threading.Channels;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Worker;

/// <summary>
/// Scheduler adapter for assignments already scheduled by a distributed coordinator.
/// </summary>
internal sealed class WorkerModuleScheduler : IModuleScheduler
{
    private static readonly Channel<ModuleState> EmptyChannel = Channel.CreateUnbounded<ModuleState>();

    private WorkerModuleScheduler()
    {
    }

    public static WorkerModuleScheduler Instance { get; } = new();

    public ChannelReader<ModuleState> ReadyModules => EmptyChannel.Reader;

    public void InitializeModules(IEnumerable<IModule> modules)
    {
    }

    public Task RunSchedulerAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public bool MarkModuleStarted(Type moduleType) => true;

    public void MarkModuleCompleted(
        Type moduleType,
        bool success,
        Exception? exception = null,
        ModuleStatus? statusOverride = null)
    {
    }

    public Task<IModule>? GetModuleCompletionTask(Type moduleType) => null;

    public ModuleState? GetModuleState(Type moduleType) => null;

    public IReadOnlyList<IModule> CancelPendingModules() => [];

    public void Dispose()
    {
    }
}
