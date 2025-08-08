using Mediator;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a submodule is created.
/// </summary>
internal record SubModuleCreatedNotification(ModuleBase ParentModule, SubModuleBase SubModule, TimeSpan EstimatedDuration) : INotification
{
    /// <summary>
    /// Gets the timestamp when the submodule was created.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}