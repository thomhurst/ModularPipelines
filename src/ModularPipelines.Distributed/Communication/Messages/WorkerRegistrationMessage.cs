using System.Text.Json.Serialization;
using ModularPipelines.Distributed.Abstractions;

namespace ModularPipelines.Distributed.Communication.Messages;

/// <summary>
/// Represents a message sent by a worker to register with the orchestrator.
/// </summary>
public sealed class WorkerRegistrationMessage
{
    /// <summary>
    /// Gets or sets the worker node information.
    /// </summary>
    [JsonPropertyName("workerNode")]
    public required WorkerNode WorkerNode { get; init; }

    /// <summary>
    /// Gets or sets the timestamp of registration.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Represents the response to a worker registration request.
/// </summary>
public sealed class WorkerRegistrationResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the registration was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public required bool Success { get; init; }

    /// <summary>
    /// Gets or sets the assigned worker ID.
    /// </summary>
    [JsonPropertyName("workerId")]
    public string? WorkerId { get; init; }

    /// <summary>
    /// Gets or sets the error message if registration failed.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Gets or sets the heartbeat interval in seconds.
    /// </summary>
    [JsonPropertyName("heartbeatIntervalSeconds")]
    public int HeartbeatIntervalSeconds { get; init; } = 30;
}
