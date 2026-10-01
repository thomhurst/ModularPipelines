using Mediator;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a sub-module is created.
/// </summary>
internal record SubModuleCreatedNotification(IModule ParentModule, SubModuleBase SubModule, TimeSpan EstimatedDuration) : INotification
{
    /// <summary>
    /// Gets the timestamp when the sub-module was created.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
