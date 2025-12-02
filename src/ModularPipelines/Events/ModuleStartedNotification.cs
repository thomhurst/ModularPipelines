using Mediator;
using ModularPipelines.Engine;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a module starts execution.
/// </summary>
internal record ModuleStartedNotification(ModuleState ModuleState, TimeSpan EstimatedDuration) : INotification
{
    /// <summary>
    /// Gets the timestamp when the module started.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}