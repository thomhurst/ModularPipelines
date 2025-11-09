using Mediator;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a module is skipped.
/// </summary>
internal record ModuleSkippedNotification(IModule Module, SkipDecision SkipDecision) : INotification
{
    /// <summary>
    /// Gets the timestamp when the module was skipped.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
