using Mediator;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a submodule completes execution.
/// </summary>
internal record SubModuleCompletedNotification(IModule ParentModule, SubModuleBase SubModule, bool IsSuccessful) : INotification
{
    /// <summary>
    /// Gets the timestamp when the submodule completed.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}