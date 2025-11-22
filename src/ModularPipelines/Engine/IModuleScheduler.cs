using System.Threading.Channels;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages eager parallel scheduling of modules using channels
/// </summary>
internal interface IModuleScheduler : IDisposable
{
    /// <summary>
    /// Gets the channel reader for consuming ready modules
    /// </summary>
    ChannelReader<ModuleState> ReadyModules { get; }

    /// <summary>
    /// Initializes module states for a collection of modules
    /// </summary>
    void InitializeModules(IEnumerable<IModule> modules);

    /// <summary>
    /// Starts the scheduler loop that continuously queues ready modules
    /// </summary>
    Task RunSchedulerAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Marks a module as started execution
    /// </summary>
    void MarkModuleStarted(Type moduleType);

    /// <summary>
    /// Marks a module as completed and notifies dependents
    /// </summary>
    void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null);

    /// <summary>
    /// Gets the completion task for a specific module
    /// </summary>
    Task<IModule>? GetModuleCompletionTask(Type moduleType);

    /// <summary>
    /// Gets the state for a specific module
    /// </summary>
    ModuleState? GetModuleState(Type moduleType);

    /// <summary>
    /// Cancels all modules that are queued or pending (not yet executing)
    /// This is used when the pipeline is cancelled to ensure TaskCompletionSources are properly completed
    /// </summary>
    void CancelPendingModules();
}
