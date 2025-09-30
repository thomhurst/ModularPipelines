using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Communication.Messages;

/// <summary>
/// Represents the response containing the result of a module execution.
/// </summary>
public sealed class ModuleResultResponse
{
    /// <summary>
    /// Gets or sets the execution ID this response is for.
    /// </summary>
    [JsonPropertyName("executionId")]
    public required string ExecutionId { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the execution was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public required bool Success { get; init; }

    /// <summary>
    /// Gets or sets the serialized module result.
    /// </summary>
    [JsonPropertyName("serializedResult")]
    public string? SerializedResult { get; init; }

    /// <summary>
    /// Gets or sets the error message if execution failed.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Gets or sets the exception type if execution failed.
    /// </summary>
    [JsonPropertyName("exceptionType")]
    public string? ExceptionType { get; init; }

    /// <summary>
    /// Gets or sets the stack trace if execution failed.
    /// </summary>
    [JsonPropertyName("stackTrace")]
    public string? StackTrace { get; init; }

    /// <summary>
    /// Gets or sets the duration of the execution.
    /// </summary>
    [JsonPropertyName("duration")]
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Gets or sets the timestamp when execution started.
    /// </summary>
    [JsonPropertyName("startTime")]
    public DateTimeOffset StartTime { get; init; }

    /// <summary>
    /// Gets or sets the timestamp when execution ended.
    /// </summary>
    [JsonPropertyName("endTime")]
    public DateTimeOffset EndTime { get; init; }

    /// <summary>
    /// Gets or sets the worker ID that executed this module.
    /// </summary>
    [JsonPropertyName("workerId")]
    public required string WorkerId { get; init; }
}
