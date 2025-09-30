using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Communication.Messages;

/// <summary>
/// Represents a message to cancel a module execution.
/// </summary>
public sealed class CancellationMessage
{
    /// <summary>
    /// Gets or sets the execution ID to cancel.
    /// </summary>
    [JsonPropertyName("executionId")]
    public required string ExecutionId { get; init; }

    /// <summary>
    /// Gets or sets the reason for cancellation.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; init; }

    /// <summary>
    /// Gets or sets the timestamp of cancellation.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Represents the response to a cancellation request.
/// </summary>
public sealed class CancellationResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the cancellation was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public required bool Success { get; init; }

    /// <summary>
    /// Gets or sets the error message if cancellation failed.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; init; }
}
