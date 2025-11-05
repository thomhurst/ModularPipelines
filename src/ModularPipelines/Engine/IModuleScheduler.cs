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
    void InitializeModules(IEnumerable<ModuleBase> modules);

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
    Task<ModuleBase>? GetModuleCompletionTask(Type moduleType);
}
