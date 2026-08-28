using Mediator;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a sub-operation completes execution.
/// </summary>
internal record SubModuleCompletedNotification(IModule ParentModule, SubModuleBase SubModule, bool IsSuccessful) : INotification
{
    /// <summary>
    /// Gets the timestamp when the sub-operation completed.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
