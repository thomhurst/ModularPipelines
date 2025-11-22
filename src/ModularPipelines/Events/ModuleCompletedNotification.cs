using Mediator;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a module completes execution.
/// </summary>
internal record ModuleCompletedNotification(IModule Module, bool IsSuccessful) : INotification
{
    /// <summary>
    /// Gets the timestamp when the module completed.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
