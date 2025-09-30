using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Communication.Messages;

/// <summary>
/// Represents a heartbeat message sent by a worker to indicate it's still alive.
/// </summary>
public sealed class HeartbeatMessage
{
    /// <summary>
    /// Gets or sets the worker ID.
    /// </summary>
    [JsonPropertyName("workerId")]
    public required string WorkerId { get; init; }

    /// <summary>
    /// Gets or sets the current load (number of executing modules).
    /// </summary>
    [JsonPropertyName("currentLoad")]
    public int CurrentLoad { get; init; }

    /// <summary>
    /// Gets or sets the timestamp of this heartbeat.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Represents the response to a heartbeat message.
/// </summary>
public sealed class HeartbeatResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the heartbeat was acknowledged.
    /// </summary>
    [JsonPropertyName("acknowledged")]
    public bool Acknowledged { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the worker should drain and shutdown.
    /// </summary>
    [JsonPropertyName("shouldDrain")]
    public bool ShouldDrain { get; init; }
}
