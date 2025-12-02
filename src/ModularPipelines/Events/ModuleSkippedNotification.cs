using Mediator;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Events;

/// <summary>
/// Notification that is published when a module is skipped.
/// </summary>
internal record ModuleSkippedNotification(ModuleState ModuleState, SkipDecision SkipDecision) : INotification
{
    /// <summary>
    /// Gets the timestamp when the module was skipped.
    /// </summary>
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}