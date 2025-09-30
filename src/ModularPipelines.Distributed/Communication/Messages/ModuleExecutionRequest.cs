using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Communication.Messages;

/// <summary>
/// Represents a request to execute a module on a worker node.
/// </summary>
public sealed class ModuleExecutionRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for this execution request.
    /// </summary>
    [JsonPropertyName("executionId")]
    public required string ExecutionId { get; init; }

    /// <summary>
    /// Gets or sets the serialized module to execute.
    /// </summary>
    [JsonPropertyName("serializedModule")]
    public required string SerializedModule { get; init; }

    /// <summary>
    /// Gets or sets the type name of the module (for deserialization).
    /// </summary>
    [JsonPropertyName("moduleTypeName")]
    public required string ModuleTypeName { get; init; }

    /// <summary>
    /// Gets or sets the serialized dependency results required by this module.
    /// </summary>
    [JsonPropertyName("dependencyResults")]
    public Dictionary<string, string> DependencyResults { get; init; } = new();

    /// <summary>
    /// Gets or sets the environment variables to set for this execution.
    /// </summary>
    [JsonPropertyName("environmentVariables")]
    public Dictionary<string, string> EnvironmentVariables { get; init; } = new();

    /// <summary>
    /// Gets or sets the working directory for this execution.
    /// </summary>
    [JsonPropertyName("workingDirectory")]
    public string? WorkingDirectory { get; init; }

    /// <summary>
    /// Gets or sets the timeout for this execution.
    /// </summary>
    [JsonPropertyName("timeout")]
    public TimeSpan? Timeout { get; init; }

    /// <summary>
    /// Gets or sets the timestamp when this request was created.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
