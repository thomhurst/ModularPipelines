using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Abstractions;

/// <summary>
/// Represents a worker node in the distributed pipeline.
/// </summary>
public sealed class WorkerNode
{
    /// <summary>
    /// Gets or sets the unique identifier for the worker node.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// Gets or sets the endpoint URL for communicating with the worker.
    /// </summary>
    [JsonPropertyName("endpoint")]
    public required string Endpoint { get; init; }

    /// <summary>
    /// Gets or sets the capabilities of the worker node.
    /// </summary>
    [JsonPropertyName("capabilities")]
    public required WorkerCapabilities Capabilities { get; init; }

    /// <summary>
    /// Gets or sets the last heartbeat timestamp.
    /// </summary>
    [JsonPropertyName("lastHeartbeat")]
    public DateTimeOffset LastHeartbeat { get; set; }

    /// <summary>
    /// Gets or sets the number of modules currently executing on this worker.
    /// </summary>
    [JsonPropertyName("currentLoad")]
    public int CurrentLoad { get; set; }

    /// <summary>
    /// Gets or sets the status of the worker node.
    /// </summary>
    [JsonPropertyName("status")]
    public WorkerStatus Status { get; set; } = WorkerStatus.Available;
}

/// <summary>
/// Represents the capabilities of a worker node.
/// </summary>
public sealed class WorkerCapabilities
{
    /// <summary>
    /// Gets or sets the operating system of the worker.
    /// </summary>
    [JsonPropertyName("os")]
    public OS Os { get; init; } = DetectCurrentOs();

    /// <summary>
    /// Gets or sets the list of tools installed on the worker.
    /// </summary>
    [JsonPropertyName("installedTools")]
    public IReadOnlyList<string> InstalledTools { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Gets or sets the maximum number of modules that can execute in parallel on this worker.
    /// </summary>
    [JsonPropertyName("maxParallelModules")]
    public int MaxParallelModules { get; init; } = Environment.ProcessorCount;

    /// <summary>
    /// Gets or sets custom tags for the worker (e.g., "gpu-enabled", "high-memory").
    /// </summary>
    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Detects the current operating system.
    /// </summary>
    /// <returns>The current OS.</returns>
    private static OS DetectCurrentOs()
    {
        if (OperatingSystem.IsWindows())
        {
            return OS.Windows;
        }

        if (OperatingSystem.IsLinux())
        {
            return OS.Linux;
        }

        if (OperatingSystem.IsMacOS())
        {
            return OS.MacOS;
        }

        return OS.Unknown;
    }
}

/// <summary>
/// Represents the status of a worker node.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WorkerStatus
{
    /// <summary>
    /// Worker is available for work.
    /// </summary>
    Available,

    /// <summary>
    /// Worker is busy executing modules.
    /// </summary>
    Busy,

    /// <summary>
    /// Worker is offline or unresponsive.
    /// </summary>
    Offline,

    /// <summary>
    /// Worker is draining and not accepting new work.
    /// </summary>
    Draining,
}
