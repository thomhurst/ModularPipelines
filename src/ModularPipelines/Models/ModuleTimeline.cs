using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Contains timing information for a single module's execution lifecycle.
/// </summary>
[ExcludeFromCodeCoverage]
public record ModuleTimeline
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    [JsonInclude]
    public string ModuleName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the full type name of the module.
    /// </summary>
    [JsonInclude]
    public string ModuleTypeName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the priority of the module.
    /// </summary>
    [JsonInclude]
    public ModulePriority Priority { get; init; }

    /// <summary>
    /// Gets the execution type hint of the module.
    /// </summary>
    [JsonInclude]
    public ExecutionType ExecutionType { get; init; }

    /// <summary>
    /// Gets when all dependencies were satisfied and the module became ready to execute.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset? ReadyTime { get; init; }

    /// <summary>
    /// Gets when the module was queued for execution.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset? QueuedTime { get; init; }

    /// <summary>
    /// Gets when the module started executing.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset? StartTime { get; init; }

    /// <summary>
    /// Gets when the module completed execution.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset? EndTime { get; init; }

    /// <summary>
    /// Gets the time spent waiting for dependencies to complete.
    /// </summary>
    [JsonInclude]
    public TimeSpan? DependencyWaitTime { get; init; }

    /// <summary>
    /// Gets the time spent in the queue waiting for an execution slot.
    /// </summary>
    [JsonInclude]
    public TimeSpan? QueueWaitTime { get; init; }

    /// <summary>
    /// Gets the actual execution duration of the module.
    /// </summary>
    [JsonInclude]
    public TimeSpan? ExecutionDuration { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module was skipped.
    /// </summary>
    [JsonInclude]
    public bool WasSkipped { get; init; }

    /// <summary>
    /// Gets a value indicating whether the module completed successfully.
    /// </summary>
    [JsonInclude]
    public bool WasSuccessful { get; init; }

    /// <summary>
    /// Gets the final status of the module.
    /// </summary>
    [JsonInclude]
    public Status Status { get; init; }
}
